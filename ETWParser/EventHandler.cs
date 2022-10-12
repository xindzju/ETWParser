using System;
using System.Collections.Generic;

using Microsoft.Windows.EventTracing;
//DataSource
using Microsoft.Windows.EventTracing.Processes; //IProcessDataSource
using Microsoft.Windows.EventTracing.Cpu; //ICpuSampleDataSource
using Microsoft.Windows.EventTracing.Symbols; //ISymbolDataSource
using Microsoft.Windows.EventTracing.Graphics;
using Microsoft.Windows.EventTracing.Metadata;
using Microsoft.Windows.EventTracing.Power;

namespace ETWParser
{
    //DXGK Engine Type: https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmdt/ne-d3dkmdt-dxgk_engine_type
    public class EventHandler
    {
        public EventHandler() { }
        public void ProcessTrace(string traceFilePath)
        {
            Logger.logger.Info("Start to process trace: {0}", traceFilePath);
            //traceprocessor
            using (ITraceProcessor trace = TraceProcessor.Create(traceFilePath))
            {
                /*
                 * built-in data sources of traceprocessor: https://docs.microsoft.com/en-us/windows/apps/trace-processing/tutorial#built-in-data-sources

                 * trace.UseCpuIdleStates()	Provides data from a trace about CPU C-states.	CPU Idle States table (when Type is Actual)
                 * trace.UseCpuSchedulingData()	Provides data from a trace about CPU thread scheduling, including context switches and ready thread events.	CPU Usage (Precise) table
                 * trace.UseDirectXData()	Provides data from a trace about DirectX activity.	GPU Utilization table
                 * traceUseDiskIOData()	Provides data from a trace about Disk I/O activity.	Disk Usage table
                 * trace.UseGenericEvents()	Provides manifested and TraceLogging events from a trace.	Generic Events table (when Event Type is Manifested or TraceLogging)
                 * trace.UseMemoryUtilizationData()	Provides data from a trace about total system memory utilization.	Memory Utilization table
                 * trace.UsePlatformIdleStates()	Provides data from a trace about the target and actual platform idle states of a system.	Platform Idle State table
                 * trace.UsePowerConfigurationData()	Provides data from a trace about system power configuration.	System Configuration, Power Settings
                 * trace.UseProcessorFrequencyData()	Provides data from a trace about the frequency at which processors ran.	Processor Frequency table (when Type is Actual)
                 * trace.UseStackEvents()	Provides data about events associated with stacks recorded during a trace.	Stacks table
                 * trace.UseSystemMetadata()	Provides general, system-wide metadata from a trace.	System Configuration
                 * trace.UseSystemSleepData()	Provides data from a trace about overall system power state.	Power Transition table
                 * trace.UseTargetCpuIdleStates()	Provides data from a trace about target CPU C-states.	CPU Idle States table (when Type is Target)
                 * trace.UseTargetProcessorFrequencyData()	Provides data from a trace about target processor frequencies.	Processor Frequency table (when Type is Target)
                 */

                /*
                 * step1: what data you want use from a trace
                 * telling the processor what kinds of data you want up front means you do not need to spend time processing large volumes of all possible kinds of trace data. 
                 * instead, TraceProcessor just does the work needed to provide the specific kinds of data you request.
                 */
                IPendingResult<IProcessDataSource> pendingProcessData = trace.UseProcesses();
                IPendingResult<ISymbolDataSource> pendingSymbolData = trace.UseSymbols();
                IPendingResult<ICpuSampleDataSource> pendingCpuSamplingData = trace.UseCpuSamplingData();

                // Processor Frequency
                IPendingResult<IProcessorFrequencyDataSource> pendingFreqData = trace.UseProcessorFrequencyData();
                // CPU Idle State
                IPendingResult<ICpuIdleStateDataSource> pendingCStateData = trace.UseCpuIdleStates();
                // CPU Scheduling
                IPendingResult<ICpuSchedulingDataSource> pendingCPUSchedulingData = trace.UseCpuSchedulingData();
                // Context Switch
                IPendingResult<IContextSwitchDataSource> pendingCSwitchData = trace.UseContextSwitchData();
                // DirectX Data
                IPendingResult<IDirectXDataSource> pendingDirectXData = trace.UseDirectXData();
                // System Metadata
                IPendingResult<ISystemMetadata> pendingSysMetadata = trace.UseSystemMetadata();

                //extend traceprocessor
                CustomDataSource customDataSource = new CustomDataSource();
                trace.Use(customDataSource); //Use: method of interface ITraceSource

                /*
                 * step2: process the trace
                 */
                trace.Process();

                /* 
                 * step3: access the result
                 */
                IProcessDataSource processData = pendingProcessData.Result;
                ISymbolDataSource symbolData = pendingSymbolData.Result;
                ICpuSampleDataSource cpuSamplingData = pendingCpuSamplingData.Result;

                IProcessorFrequencyDataSource freqData = pendingFreqData.Result;
                ICpuIdleStateDataSource cStateData = pendingCStateData.Result;
                ICpuSchedulingDataSource schedulingData = pendingCPUSchedulingData.Result;
                IContextSwitchDataSource cswitchData = pendingCSwitchData.Result;
                IDirectXDataSource directXData = pendingDirectXData.Result;
                ISystemMetadata sysMetadata = pendingSysMetadata.Result;

                //Frequency

                //Idle State

                // CPU utilization
                foreach (ICpuTimeSlice cpuTimeSlice in schedulingData.CpuTimeSlices)
                {
                    CSwitchEvent CSEvent = new CSwitchEvent(cpuTimeSlice);
                }
                
                //DMA Packet

                //Queue Packet
            }
            Logger.logger.Info("Process trace done");
        }

        class CustomDataSource : IFilteredEventConsumer
        {
            //802ec45a-1e99-4b83-9920-87c98277ba9d: Microsoft-Windows-DxgKrnl
            public IReadOnlyList<Guid> ProviderIds { get; } = new Guid[] { new Guid("802ec45a-1e99-4b83-9920-87c98277ba9d") };

            // Count can be retrieved throughout the application but only set from the class containing it
            public int Count { get; private set; }

            // EventContext: the event to process
            public void Process(EventContext eventContext)
            {
                ++Count;
            }
        }
    }
}
