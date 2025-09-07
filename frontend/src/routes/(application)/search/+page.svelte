<script lang="ts">
	import * as Card from "$lib/components/ui/card";
	import Button from "$lib/components/ui/button/button.svelte";
	import { Search } from "lucide-svelte";

	let query = "";
	let results = [
		{ id: 1, title: "teacher-v2 model", desc: "Uploaded 2025-09-01" },
		{ id: 2, title: "distillation job #342", desc: "Running — 12%" },
	];

	function doSearch() {
		console.log('search', query);
	}
</script>

<div class="space-y-4 px-4 lg:px-6">
	<Card.Root>
		<Card.Header>
			<div class="flex items-center gap-3">
				<Search class="size-5 text-primary" />
				<Card.Title>Search</Card.Title>
			</div>
			<Card.Action>
				<Button variant="outline" size="sm" onclick={doSearch}>Search</Button>
			</Card.Action>
		</Card.Header>
		<Card.Content>
			<div class="flex gap-2">
				<input
					class="flex-1 rounded-md border px-3 py-2"
					placeholder="Search models, jobs, users..."
					bind:value={query}
				/>
				<Button variant="ghost" onclick={doSearch}><Search class="size-4" /></Button>
			</div>

			<div class="mt-4 space-y-3">
				{#each results as r}
					<Card.Root class="bg-background/50">
						<Card.Header>
							<Card.Title>{r.title}</Card.Title>
							<Card.Action>
								<Button variant="ghost" size="sm">Open</Button>
							</Card.Action>
						</Card.Header>
						<Card.Content>
							<Card.Description>{r.desc}</Card.Description>
						</Card.Content>
					</Card.Root>
				{/each}
			</div>
		</Card.Content>
	</Card.Root>
</div>
