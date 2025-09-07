<script lang="ts">
  import { onMount } from "svelte";
  import privacyMd from "$lib/assets/privacy/privacy.en.md?raw";
  import * as Card from "$lib/components/ui/card";
  import Button from "$lib/components/ui/button/button.svelte";

  let carta: any = null;
  let MarkdownComponent: any = null;
  let value = "...";

  onMount(async () => {
    const mod = await import("carta-md");
    const { Carta, Markdown } = mod;
    carta = new Carta();
    MarkdownComponent = Markdown;
    value = privacyMd;
  });
</script>

{#if carta && MarkdownComponent}
  <Card.Root class="max-w-4xl mx-auto my-12">
    <Card.Header>
      <Card.Title>Privacy Policy</Card.Title>
      <Card.Action>
        <Button onclick={() => history.back()} variant="ghost">Back</Button>
      </Card.Action>
    </Card.Header>
    <Card.Content>
      <article class="prose prose-sm md:prose-base lg:prose-lg xl:prose-xl 2xl:prose-2xl p-3 prose-zinc carta-theme__default">
        <svelte:component this={MarkdownComponent} {carta} {value} theme="default" />
      </article>
    </Card.Content>
  </Card.Root>
{:else}
  <div class="flex items-center justify-center h-40">Loading…</div>
{/if}
