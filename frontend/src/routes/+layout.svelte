<script lang="ts">
	import "../app.css";
	import favicon from "$lib/assets/favicon.png";
	import { onMount } from "svelte";
	import { goto } from "$app/navigation";
	import { page } from "$app/state";
	import * as Sidebar from "$lib/components/ui/sidebar/index.js";
	import SiteHeader from "$lib/components/site-header.svelte";
	import AppSidebar from "$lib/components/app-sidebar.svelte";
	let { children } = $props();

	const publicRoutes = ["/login", "/", "/privacy", "/terms"];

	const isPublicRoute = $derived(() =>
		publicRoutes.includes(page.url.pathname),
	);

	onMount(() => {
		if (typeof window !== "undefined" && window.location.pathname === "/") {
			if (localStorage.getItem("accessToken")) {
				goto("/home");
			} else {
				goto("/login", { replaceState: true });
			}
		}
	});
</script>

<svelte:head>
	<link rel="icon" href={favicon} />
	<title>Dotnet Svelte Admin</title>
</svelte:head>

{#if isPublicRoute()}
	{@render children?.()}
{:else}
	<Sidebar.Provider
		style="--sidebar-width: calc(var(--spacing) * 72); --header-height: calc(var(--spacing) * 12);"
	>
		<AppSidebar variant="inset" />
		<Sidebar.Inset>
			<SiteHeader />
			<div class="flex flex-1 flex-col">
				<div class="@container/main flex flex-1 flex-col gap-2">
					<div class="flex flex-col gap-4 py-4 md:gap-6 md:py-6">
						{@render children?.()}
					</div>
				</div>
			</div>
		</Sidebar.Inset>
	</Sidebar.Provider>
{/if}
