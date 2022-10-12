using Microsoft.Windows.EventTracing.Cpu;
using System;

namespace ETWParser
{
    public enum EventType
    {
        TEMPORAL = 0,
        RANGE
    };

    public struct EventMetaData
    {
        public string? processName; //default: null
        public uint? processID; //default: null
        public string? threadName; //default: null
        public uint? threadID; //default: null
        public uint? cpuCore; //default: null
        public string? desc; //default: null
    }

    public abstract class BaseEvent
    {
        protected BaseEvent(string eventName, EventMetaData eventMetaData)
        {
            this.eventName = eventName;
            this.eventMetaData = new EventMetaData();
            this.eventMetaData.processName = eventMetaData.processName;
            this.eventMetaData.processID = eventMetaData.processID;
            this.eventMetaData.threadName = eventMetaData.threadName;
            this.eventMetaData.threadID = eventMetaData.threadID;
            this.eventMetaData.desc = eventMetaData.desc;
        }

        public void SetEventName(string eventName)
        {
            this.eventName = eventName;
        }

        public string GetEventName()
        {
            return this.eventName;
        }

        public void SetEventType(EventType eventType)
        {
            this.eventType = eventType;
        }

        public EventType GetEventType()
        {
            return this.eventType;
        }

        public void SetEventMetaData(EventMetaData eventMetaData)
        {
            this.eventMetaData.processName = eventMetaData.processName;
            this.eventMetaData.processID = eventMetaData.processID;
            this.eventMetaData.threadName = eventMetaData.threadName;
            this.eventMetaData.threadID = eventMetaData.threadID;
            this.eventMetaData.desc = eventMetaData.desc;
        }

        public EventMetaData GetEventMetaData()
        {
            return this.eventMetaData;
        }

        protected string eventName;
        protected EventType eventType;
        protected EventMetaData eventMetaData;
    }

    public class TemporalEvent : BaseEvent
    {
        public TemporalEvent(string eventName, ulong eventTimestamp, EventMetaData eventMetaData) : base(eventName, eventMetaData)
        {
            this.eventTimestamp = eventTimestamp;
            this.eventType = EventType.TEMPORAL;
        }

        public void SetEventTimestamp(ulong eventTimestamp)
        {
            this.eventTimestamp = eventTimestamp;
        }

        public ulong GetEventTimestamp()
        {
            return this.eventTimestamp;
        }

        protected ulong eventTimestamp;
    }

    public class RangeEvent : BaseEvent
    {
        public RangeEvent(string eventName, ulong eventStartTimestamp, ulong eventStopTimestamp, EventMetaData eventMetaData) : base(eventName, eventMetaData)
        {
            this.eventStartTimestamp = eventStartTimestamp;
            this.eventStopTimestamp = eventStopTimestamp;
            this.eventType = EventType.RANGE;
        }
        public void SetEventTimestamp(ulong eventStartTimestamp, ulong eventStopTimestamp)
        {
            this.eventStartTimestamp = eventStartTimestamp;
            this.eventStopTimestamp = eventStopTimestamp;
        }
        public (ulong, ulong) GetEventTimestamp()
        {
            return (this.eventStartTimestamp, this.eventStopTimestamp);
        }

        protected ulong eventStartTimestamp;
        protected ulong eventStopTimestamp;
    }

    public class CSwitchEvent
    {
        public CSwitchEvent(ICpuTimeSlice cpuTimeSlice)
        {
            this.m_cpuTimeSlice = cpuTimeSlice;
            this.InitEvent();
        }

        private void InitEvent()
        {
            string eventName = "CSwitch";
            ulong eventStartTimestamp = ((ulong)m_cpuTimeSlice.StartTime.RelativeTimestamp.Nanoseconds);
            ulong eventStopTimestamp = ((ulong)m_cpuTimeSlice.StopTime.RelativeTimestamp.Nanoseconds);
            uint cpuCore = ((uint)m_cpuTimeSlice.Processor);
            string processName = m_cpuTimeSlice.Process.ImageName;
            uint processID = ((uint)m_cpuTimeSlice.Process.Id);
            string threadName = m_cpuTimeSlice.Thread.Name;
            uint threadID = ((uint)m_cpuTimeSlice.Thread.Id);

            EventMetaData metadata = new EventMetaData();
            metadata.processName = processName;
            metadata.processID = processID;
            metadata.cpuCore = cpuCore;
            metadata.threadName = threadName;
            metadata.threadID = threadID;
            metadata.desc = "";

            //debug purpose
            Console.WriteLine($"CSwitch, pName: {processName}, pID: {processID}, cpuCore: {cpuCore}, tName: {threadName}, tID: {threadID}");

            m_event = new RangeEvent(eventName, eventStartTimestamp, eventStopTimestamp, metadata);
        }

        ICpuTimeSlice m_cpuTimeSlice;
        public RangeEvent m_event;
    }
}

