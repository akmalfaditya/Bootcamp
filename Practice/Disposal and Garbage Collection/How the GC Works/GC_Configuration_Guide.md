# Runtime Configuration Options for GC Tuning

This file documents the GC configuration options available in `runtimeconfig.template.json`.

## GC Mode Configuration

### System.GC.Server
- **Values**: `true` or `false`
- **Default**: `false` (Workstation GC)
- **Workstation GC**: Single heap, single GC thread, optimized for responsiveness
- **Server GC**: Multiple heaps (one per core), multiple GC threads, optimized for throughput
- **When to use Server GC**: Multi-core server applications prioritizing throughput over latency

### System.GC.Concurrent
- **Values**: `true` or `false`
- **Default**: `true`
- **Purpose**: Enables background GC collections that run concurrently with application
- **Benefit**: Reduces GC pause times significantly
- **Trade-off**: Slightly higher CPU and memory usage

## Memory Management

### System.GC.RetainVM
- **Values**: `true` or `false`
- **Default**: `false`
- **Purpose**: Controls whether GC returns memory to the operating system
- **true**: Retains virtual memory (faster allocations, higher memory usage)
- **false**: Returns memory to OS when possible (lower memory footprint)

### System.GC.LOHThreshold
- **Values**: Integer (bytes)
- **Default**: `85000` (85KB)
- **Purpose**: Size threshold for Large Object Heap placement
- **Impact**: Objects ≥ this size go to LOH (no compaction, different collection behavior)
- **Typical range**: 85,000 to 1,000,000 bytes

## Performance Tuning

### System.GC.LatencyMode
- **Values**: 
  - `0`: Batch (maximize throughput)
  - `1`: Interactive (balance latency and throughput) - Default
  - `2`: LowLatency (minimize pause times)
  - `3`: SustainedLowLatency (consistently low latency)
- **Use cases**:
  - Batch: Background processing, data analytics
  - Interactive: Most applications
  - LowLatency: Real-time applications, games
  - SustainedLowLatency: High-frequency trading, audio processing

### System.GC.HeapCount
- **Values**: `0` (auto) or positive integer
- **Default**: `0` (automatic based on CPU cores)
- **Purpose**: Controls number of GC heaps in Server GC mode
- **Recommendation**: Usually leave at 0 for automatic optimization

### System.GC.NoAffinitize
- **Values**: `true` or `false`
- **Default**: `false`
- **Purpose**: Controls GC thread CPU affinity
- **true**: GC threads can run on any CPU
- **false**: GC threads are affinitized to specific CPUs for better cache locality

## Memory Limits (Container-Friendly)

### System.GC.HeapHardLimit
- **Values**: Integer (bytes) or `0` (disabled)
- **Default**: `0`
- **Purpose**: Sets absolute memory limit for GC heap
- **Use case**: Containers with strict memory limits
- **Example**: `1073741824` for 1GB limit

### System.GC.HeapHardLimitPercent
- **Values**: Integer (percentage) or `0` (disabled)
- **Default**: `0`
- **Purpose**: Sets heap limit as percentage of available memory
- **Use case**: Containers where memory is dynamically allocated
- **Example**: `75` for 75% of available memory

### System.GC.HighMemoryPercent
- **Values**: Integer (percentage)
- **Default**: `90`
- **Purpose**: Memory threshold to trigger more aggressive GC
- **Impact**: GC becomes more aggressive above this memory usage percentage
- **Typical range**: 70-95%

## Memory Conservation

### System.GC.ConserveMemory
- **Values**: `0` to `9`
- **Default**: `0` (no conservation)
- **Purpose**: Enables memory conservation strategies
- **Higher values**: More aggressive memory conservation at cost of performance
- **Use cases**: Memory-constrained environments, embedded systems

## Practical Configuration Examples

### High-Throughput Server
```json
{
  "System.GC.Server": true,
  "System.GC.Concurrent": true,
  "System.GC.LatencyMode": 0,
  "System.GC.RetainVM": true
}
```

### Low-Latency Application
```json
{
  "System.GC.Server": false,
  "System.GC.Concurrent": true,
  "System.GC.LatencyMode": 2,
  "System.GC.ConserveMemory": 0
}
```

### Container Deployment
```json
{
  "System.GC.Server": true,
  "System.GC.HeapHardLimitPercent": 75,
  "System.GC.HighMemoryPercent": 80,
  "System.GC.ConserveMemory": 2
}
```

### Memory-Constrained Environment
```json
{
  "System.GC.Server": false,
  "System.GC.RetainVM": false,
  "System.GC.ConserveMemory": 5,
  "System.GC.HighMemoryPercent": 70
}
```

## Testing Different Configurations

To test different configurations:

1. Modify `runtimeconfig.template.json`
2. Run `dotnet build` to apply changes
3. Run `dotnet run` to see the impact
4. Compare GC behavior, collection counts, and memory usage

## Monitoring GC Performance

Use these APIs in your code to monitor configuration impact:
- `GCSettings.IsServerGC` - Check current GC mode
- `GC.CollectionCount(generation)` - Monitor collection frequency
- `GC.GetTotalMemory(false)` - Track memory usage
- `GC.GetGCMemoryInfo()` - Detailed GC statistics

## Best Practices

1. **Start with defaults** - Only change when you have specific performance requirements
2. **Measure impact** - Use profiling tools to verify configuration benefits
3. **Test under load** - GC behavior changes significantly under different allocation patterns
4. **Consider your application type** - Server vs desktop vs container environments have different optimal settings
5. **Monitor in production** - GC performance can vary with real-world usage patterns
