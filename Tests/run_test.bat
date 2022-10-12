xperf -on PROC_THREAD+CSWITCH+POWER -BufferSize 1024 -MinBuffers 120 -MaxBuffers 480  -f Kernel.etl
xperf -start SchedulingLog -on 802ec45a-1e99-4b83-9920-87c98277ba9d -BufferSize 1024 -MinBuffers 120 -MaxBuffers 480 -f SchedulingLog.etl
timeout /t 2
xperf -stop
xperf -stop SchedulingLog
xperf -merge Kernel.etl SchedulingLog.etl Merged.etl
xperf -i Merged.etl -o Merged.txt -a dumper
