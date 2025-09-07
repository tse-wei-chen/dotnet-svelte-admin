<script lang="ts">
  import { goto } from "$app/navigation";
  import * as Card from "$lib/components/ui/card";
  import Button from "$lib/components/ui/button/button.svelte";
  import Input from "$lib/components/ui/input/input.svelte";
  import { User, Cpu, Settings as SettingsIcon, ChartNoAxesColumn, Clock, Search, CircleQuestionMark, FilePlus, Activity } from "lucide-svelte";
  import * as Chart from "$lib/components/ui/chart/index.js";
  import { scaleBand } from "d3-scale";
  import { BarChart } from "layerchart";

  const pages = [
    { title: "Teacher models", desc: "Manage teacher models.", href: "/teacher", icon: User },
    { title: "Distillation", desc: "Create and monitor distillation jobs.", href: "/distillation", icon: Cpu },
    { title: "Analytics", desc: "Key metrics for models and jobs.", href: "/analytics", icon: ChartNoAxesColumn },
    { title: "Reports", desc: "Export and inspect distillation reports.", href: "/reports", icon: FilePlus },
    { title: "Activity", desc: "System and user activity logs.", href: "/activity", icon: Activity },
    { title: "Search", desc: "Quick search models, jobs and users.", href: "/search", icon: Search },
    { title: "Help", desc: "Get help and contact support.", href: "/help", icon: CircleQuestionMark },
    { title: "Settings", desc: "Application and account settings.", href: "/settings", icon: SettingsIcon },
  ];

  const stats = [
    { label: "Models", value: "12", icon: ChartNoAxesColumn },
    { label: "Jobs", value: "3 running", icon: Clock },
    { label: "Users", value: "24", icon: User },
  ];

  const frameworkDist = [
    { framework: "PyTorch", pct: 52 },
    { framework: "ONNX", pct: 28 },
    { framework: "TensorFlow", pct: 20 }
  ];

  const frameworkConfig = {
    pct: { label: "Share", color: "#34d399" }
  } satisfies Chart.ChartConfig;

  let q = "";
  function onSearch() {
    if (!q) return goto('/search');
    goto(`/search?q=${encodeURIComponent(q)}`);
  }
</script>

<div class="space-y-6 px-4 lg:px-6">
  <Card.Root>
    <Card.Header>
      <div class="flex items-center justify-between w-full">
        <div>
          <h1 class="text-2xl font-semibold">AI Distillation SaaS</h1>
          <p class="text-sm text-muted-foreground">MVP dashboard — upload teacher models, create distillation jobs and monitor results.</p>
        </div>
        <div class="flex items-center gap-2">
          <Button href="/distillation" variant="default" size="sm">New Task</Button>
          <Button href="/teacher" variant="outline" size="sm">Upload Model</Button>
        </div>
      </div>
    </Card.Header>
    <Card.Content>
      <div class="flex gap-3 items-center mt-3">
        <Input bind:value={q} placeholder="Search models, jobs, users..." class="flex-1" />
        <Button onclick={onSearch} variant="default">Search</Button>
      </div>
    </Card.Content>
  </Card.Root>

  <!-- Feature cards -->
  <div class="grid grid-cols-1 gap-4 md:grid-cols-3">
    {#each pages as page}
      <Card.Root class="hover:scale-[1.01] transition-transform">
        <Card.Header>
          <div class="flex items-center gap-3 w-full">
            <div class="flex items-center gap-3">
              <svelte:component this={page.icon} class="size-5 text-primary" />
              <Card.Title>{page.title}</Card.Title>
            </div>
            <Card.Action>
              <Button href={page.href} variant="outline" size="sm">Open</Button>
            </Card.Action>
          </div>
        </Card.Header>
        <Card.Content>
          <Card.Description>{page.desc}</Card.Description>
        </Card.Content>
      </Card.Root>
    {/each}
  </div>

  <!-- Quick overview + Recent activity -->
  <div class="mt-2 grid grid-cols-1 gap-4 md:grid-cols-3">
    <Card.Root class="md:col-span-2">
      <Card.Header>
        <div class="flex items-center justify-between w-full">
          <div class="flex items-center gap-3">
            <ChartNoAxesColumn class="size-5 text-primary" />
            <Card.Title>Quick overview</Card.Title>
          </div>
          <Card.Action>
            <Button href="/reports" variant="ghost" size="sm">View reports</Button>
          </Card.Action>
        </div>
      </Card.Header>
      <Card.Content>
        <div class="grid grid-cols-3 gap-4">
          {#each stats as s}
            <div class="flex flex-col items-start gap-1">
              <div class="flex items-center gap-2 text-sm text-muted-foreground">
                <svelte:component this={s.icon} class="size-4" />
                <span>{s.label}</span>
              </div>
              <div class="text-lg font-semibold">{s.value}</div>
            </div>
          {/each}
        </div>

        <!-- small framework distribution chart -->
        <div class="mt-4">
          <div class="text-xs text-muted-foreground mb-2">Model frameworks</div>
          <div class="h-28">
            <Chart.Container config={frameworkConfig} class="max-h-28 w-full">
              <BarChart
                data={frameworkDist}
                xScale={scaleBand().padding(0.4)}
                x="framework"
                axis="x"
                seriesLayout="group"
                tooltip={false}
                series={[{ key: "pct", label: frameworkConfig.pct.label, color: frameworkConfig.pct.color }]}
              />
            </Chart.Container>
          </div>
        </div>
      </Card.Content>
    </Card.Root>

    <Card.Root>
      <Card.Header>
        <div class="flex items-center gap-3">
          <Clock class="size-5 text-primary" />
          <Card.Title>Recent activity</Card.Title>
        </div>
      </Card.Header>
      <Card.Content>
        <ul class="flex flex-col gap-3">
          <li class="text-sm">Model <strong>teacher-v2</strong> finished training 2h ago.</li>
          <li class="text-sm">Distillation job <strong>#342</strong> queued.</li>
          <li class="text-sm">New user <strong>alice@example.com</strong> registered.</li>
        </ul>
        <div class="mt-4 flex justify-end">
          <Button href="/activity" variant="link" size="sm">View all</Button>
        </div>
      </Card.Content>
    </Card.Root>
  </div>
</div>
