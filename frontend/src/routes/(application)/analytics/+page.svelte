<script lang="ts">
  import * as Card from "$lib/components/ui/card";
  import Button from "$lib/components/ui/button/button.svelte";
  import { TrendingUp, Cpu, Database } from "lucide-svelte";
  import * as Chart from "$lib/components/ui/chart/index.js";
  import { scaleBand } from "d3-scale";
  import { BarChart } from "layerchart";

  const throughput = [
    { bucket: "t-6", req: 120 },
    { bucket: "t-5", req: 150 },
    { bucket: "t-4", req: 170 },
    { bucket: "t-3", req: 140 },
    { bucket: "t-2", req: 190 },
    { bucket: "t-1", req: 210 },
    { bucket: "now", req: 230 }
  ];

  const queueByWorker = [
    { worker: "gpu-1", jobs: 8 },
    { worker: "gpu-2", jobs: 5 },
    { worker: "cpu-1", jobs: 2 },
    { worker: "cpu-2", jobs: 1 }
  ];

  const accuracyCompare = [
    { model: "teacher-v2", before: 94.2, after: 91.1 },
    { model: "teacher-small", before: 90.0, after: 88.5 },
    { model: "teacher-micro", before: 86.4, after: 84.0 }
  ];

  const frameworkDist = [
    { framework: "PyTorch", pct: 52 },
    { framework: "ONNX", pct: 28 },
    { framework: "TensorFlow", pct: 20 }
  ];

  const throughputConfig = {
    req: { label: "Requests/min", color: "#2563eb" }
  } satisfies Chart.ChartConfig;
  const queueConfig = {
    jobs: { label: "Queued jobs", color: "#fb923c" }
  } satisfies Chart.ChartConfig;
  const accuracyConfig = {
    before: { label: "Before", color: "#60a5fa" },
    after: { label: "After", color: "#2563eb" }
  } satisfies Chart.ChartConfig;
  const frameworkConfig = {
    pct: { label: "Share", color: "#34d399" }
  } satisfies Chart.ChartConfig;
</script>

<div class="space-y-4 px-4 lg:px-6">
  <Card.Root>
    <Card.Header>
      <div class="flex items-center gap-3">
        <TrendingUp class="size-5 text-primary" />
        <Card.Title>Analytics — AI distillation SaaS MVP</Card.Title>
      </div>
      <Card.Action>
        <Button href="/analytics" variant="outline" size="sm">Refresh</Button>
      </Card.Action>
    </Card.Header>
    <Card.Content>
      <p class="text-sm text-muted-foreground">
        focusing on key metrics for distillation task performance, queueing, and
        model quality (MVP priority).
      </p>
      <div class="mt-4 grid grid-cols-2 gap-4">
        <div class="p-3 bg-muted/5 rounded-md">
          <div class="text-xs text-muted-foreground">
            Active distillation jobs
          </div>
          <div class="text-lg font-semibold">6</div>
        </div>
        <div class="p-3 bg-muted/5 rounded-md">
          <div class="text-xs text-muted-foreground">Models stored</div>
          <div class="text-lg font-semibold">14</div>
        </div>
      </div>
    </Card.Content>
  </Card.Root>

  <div class="mt-6 grid grid-cols-1 gap-4 px-4 lg:px-6 md:grid-cols-3">
    <!-- Throughput (time series) -->
    <Card.Root>
      <Card.Header>
        <div class="flex items-center gap-3">
          <TrendingUp class="size-5 text-primary" />
          <Card.Title>Throughput (requests / min)</Card.Title>
        </div>
      </Card.Header>
      <Card.Content>
        <div class="h-50">
          <Chart.Container
            config={throughputConfig}
            class="min-h-[200px] w-full"
          >
            <BarChart
              data={throughput}
              xScale={scaleBand().padding(0.15)}
              x="bucket"
              axis="x"
              seriesLayout="group"
              tooltip={false}
              series={[
                {
                  key: "req",
                  label: throughputConfig.req.label,
                  color: throughputConfig.req.color
                }
              ]}
            />
          </Chart.Container>
        </div>
        <div class="mt-2 text-xs text-muted-foreground">
          Showing the last 7 time buckets
        </div>
      </Card.Content>
    </Card.Root>

    <!-- Queue by worker -->
    <Card.Root>
      <Card.Header>
        <div class="flex items-center gap-3">
          <Cpu class="size-5 text-primary" />
          <Card.Title>Job queue (per worker)</Card.Title>
        </div>
      </Card.Header>
      <Card.Content>
        <div class="h-50">
          <Chart.Container config={queueConfig} class="min-h-[200px] w-full">
            <BarChart
              data={queueByWorker}
              xScale={scaleBand().padding(0.25)}
              x="worker"
              axis="x"
              seriesLayout="group"
              tooltip={false}
              series={[
                {
                  key: "jobs",
                  label: queueConfig.jobs.label,
                  color: queueConfig.jobs.color
                }
              ]}
            />
          </Chart.Container>
        </div>
        <div class="mt-2 text-xs text-muted-foreground">
          Queue length helps identify resource bottlenecks
        </div>
      </Card.Content>
    </Card.Root>

    <!-- Accuracy before/after (grouped bars) -->
    <Card.Root>
      <Card.Header>
        <div class="flex items-center gap-3">
          <Database class="size-5 text-primary" />
          <Card.Title>Accuracy before/after (grouped bars)</Card.Title>
        </div>
      </Card.Header>
      <Card.Content>
        <div class="h-50">
          <Chart.Container config={accuracyConfig} class="min-h-[200px] w-full">
            <BarChart
              data={accuracyCompare}
              xScale={scaleBand().padding(0.25)}
              x="model"
              axis="x"
              seriesLayout="group"
              tooltip={false}
              series={[
                {
                  key: "before",
                  label: accuracyConfig.before.label,
                  color: accuracyConfig.before.color
                },
                {
                  key: "after",
                  label: accuracyConfig.after.label,
                  color: accuracyConfig.after.color
                }
              ]}
            />
          </Chart.Container>
        </div>
        <div class="mt-2 text-xs text-muted-foreground">
          Showing the impact of distillation on accuracy (MVP needs to observe
          trade-offs)
        </div>
      </Card.Content>
    </Card.Root>
  </div>

  <!-- Framework distribution (simple bar breakdown) -->
  <div class="mt-4 px-4 lg:px-6">
    <Card.Root>
      <Card.Header>
        <div class="flex items-center gap-3">
          <Card.Title>Model framework distribution</Card.Title>
        </div>
      </Card.Header>
      <Card.Content>
        <div class="grid gap-2">
          {#each frameworkDist as f}
            <div class="flex items-center justify-between gap-4">
              <div class="text-sm">{f.framework}</div>
              <div class="w-2/3 bg-muted/10 rounded h-3">
                <div
                  class="h-3 rounded bg-accent"
                  style="width: {f.pct}%"
                ></div>
              </div>
              <div class="text-sm text-muted-foreground">{f.pct}%</div>
            </div>
          {/each}
        </div>
      </Card.Content>
    </Card.Root>
  </div>
</div>
