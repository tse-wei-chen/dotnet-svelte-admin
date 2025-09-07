<script lang="ts">
	import * as Card from "$lib/components/ui/card";
	import Button from "$lib/components/ui/button/button.svelte";
	import { Clock, List, Activity } from "lucide-svelte";
</script>

<div class="space-y-4 px-4 lg:px-6">
	<Card.Root>
		<Card.Header>
			<div class="flex items-center gap-3">
				<Clock class="size-5 text-primary" />
				<Card.Title>Activity / Logs</Card.Title>
			</div>
			<Card.Action>
				<Button href="/activity" variant="outline" size="sm">Refresh</Button>
			</Card.Action>
		</Card.Header>
		<Card.Content>
			<p class="text-sm text-muted-foreground">Recent system and user activity. Useful for monitoring tasks and troubleshooting.</p>
			<ul class="mt-4 space-y-2">
				<li class="text-sm">[2h ago] Model <strong>teacher-v2</strong> finished training.</li>
				<li class="text-sm">[30m ago] Distillation job <strong>#342</strong> started.</li>
				<li class="text-sm">[5m ago] User <strong>bob@example.com</strong> uploaded a model.</li>
			</ul>
		</Card.Content>
	</Card.Root>
</div>
