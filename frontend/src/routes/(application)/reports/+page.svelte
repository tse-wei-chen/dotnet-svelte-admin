<script lang="ts">
  import * as Card from "$lib/components/ui/card";
  import Button from "$lib/components/ui/button/button.svelte";
  import { BarChart2, Download } from "lucide-svelte";
</script>

<div class="space-y-4 px-4 lg:px-6">
  <Card.Root>
    <Card.Header>
      <div class="flex items-center gap-3">
        <BarChart2 class="size-5 text-primary" />
        <Card.Title>Reports & Results</Card.Title>
      </div>
      <Card.Action>
        <Button variant="outline" size="sm">Export</Button>
      </Card.Action>
    </Card.Header>
    <Card.Content>
      <p class="text-sm text-muted-foreground">
        View model performance, distillation comparisons, and export results.
      </p>
      <div class="mt-4 space-y-3">
        <div class="flex items-center justify-between">
          <div>
            <div class="text-sm">Last run</div>
            <div class="text-xs text-muted-foreground">
              teacher-v2 → student-lite (2025-09-07)
            </div>
          </div>
          <div class="flex items-center gap-2">
            <Button variant="ghost" size="sm">View</Button>
            <Button variant="outline" size="sm">
              Download
              <Download class="size-4" />
            </Button>
          </div>
        </div>
      </div>
    </Card.Content>
  </Card.Root>
</div>
