
<script lang="ts">
	import * as Card from "$lib/components/ui/card";
	import Button from "$lib/components/ui/button/button.svelte";
	import { User, Settings as SettingsIcon } from "lucide-svelte";
</script>

<div class="space-y-4 px-4 lg:px-6">
	<Card.Root>
		<Card.Header>
			<div class="flex items-center gap-3">
				<SettingsIcon class="size-5 text-primary" />
				<Card.Title>Account & Application Settings</Card.Title>
			</div>
			<Card.Action>
				<Button href="/settings" variant="outline" size="sm">Edit</Button>
			</Card.Action>
		</Card.Header>
		<Card.Content>
			<p class="text-sm text-muted-foreground">Manage account, quotas, and application preferences.</p>
			<div class="mt-4 grid grid-cols-1 gap-3">
				<div class="flex items-center justify-between">
					<div>
						<div class="text-sm">Profile</div>
						<div class="text-xs text-muted-foreground">Change your name, email and password.</div>
					</div>
					<Button href="/settings" variant="ghost" size="sm">Edit</Button>
				</div>

				<div class="flex items-center justify-between">
					<div>
						<div class="text-sm">Resource Quotas</div>
						<div class="text-xs text-muted-foreground">Manage task limits and storage quotas.</div>
					</div>
					<Button href="/settings" variant="ghost" size="sm">View</Button>
				</div>
			</div>
		</Card.Content>
	</Card.Root>
</div>
