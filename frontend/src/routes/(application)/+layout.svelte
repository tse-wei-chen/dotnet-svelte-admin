<script lang="ts">
  import { onMount } from 'svelte';
  import { browser } from '$app/environment';
  import { goto } from '$app/navigation';

  // Use the project's reactive state helper when available
  let { children } = $props();
  let ready = $state(false);

  onMount(() => {
    if (!browser) return;

    const userinfo = localStorage.getItem('userinfo');
    const accessToken = localStorage.getItem('accessToken');

    if (userinfo && accessToken) {
      // Client is authenticated, allow rendering of children
      ready = true;
    } else {
      // Not authenticated - redirect to login
      goto('/login');
    }
  });
</script>

{#if ready}
  {@render children?.()}
{:else}
  <!-- minimal placeholder to avoid layout flash while we check auth -->
  <div aria-hidden="true" class="w-full h-full"></div>
{/if}

