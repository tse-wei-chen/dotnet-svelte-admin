<script lang="ts">
	import { Button } from '$lib/components/ui/button';
	import { Sun, Moon } from 'lucide-svelte';
	import { browser } from '$app/environment';
	import { onMount } from 'svelte';
	
	let isDark = false;
	
	onMount(() => {
		if (browser) {
			const savedTheme = localStorage.getItem('theme');
			const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
			
			isDark = savedTheme === 'dark' || (!savedTheme && prefersDark);
			applyTheme();
		}
	});
	
	function toggleTheme() {
		isDark = !isDark;
		if (browser) {
			localStorage.setItem('theme', isDark ? 'dark' : 'light');
			applyTheme();
		}
	}
	
	function applyTheme() {
		if (browser) {
			if (isDark) {
				document.documentElement.classList.add('dark');
			} else {
				document.documentElement.classList.remove('dark');
			}
		}
	}
</script>

<Button 
	variant="outline" 
	size="icon"
	onclick={toggleTheme}
	aria-label="Toggle theme"
    class="rounded-md w-8 h-8 cursor-pointer"
>
	{#if isDark}
		<Sun class="h-4 w-4" />
	{:else}
		<Moon class="h-4 w-4" />
	{/if}
</Button>
