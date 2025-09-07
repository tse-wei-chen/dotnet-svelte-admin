<script lang="ts">
  import * as Card from "$lib/components/ui/card";
  import Button from "$lib/components/ui/button/button.svelte";
  import { CircleQuestionMark, Mail, MessageSquare } from "lucide-svelte";

  const faqs = [
    {
      q: "How do I upload a model?",
      a: "Go to Models → Upload and select your file (PyTorch/TensorFlow/ONNX)."
    },
    {
      q: "What formats are supported?",
      a: "We accept .pt, .pth, .onnx, and TensorFlow SavedModel (zipped)."
    }
  ];
</script>

<div class="space-y-4 px-4 lg:px-6">
  <Card.Root>
    <Card.Header>
      <div class="flex items-center gap-3">
        <CircleQuestionMark class="size-5 text-primary" />
        <Card.Title>Help & Support</Card.Title>
      </div>
      <Card.Action>
        <Button href="/gethelp/contact" variant="outline" size="sm"
          >Contact</Button
        >
      </Card.Action>
    </Card.Header>
    <Card.Content>
      <p class="text-sm text-muted-foreground">
        Common questions and ways to reach support.
      </p>
      <div class="mt-4 space-y-3">
        {#each faqs as f}
          <div>
            <div class="text-sm font-medium">{f.q}</div>
            <div class="text-xs text-muted-foreground">{f.a}</div>
          </div>
        {/each}
      </div>
    </Card.Content>
  </Card.Root>

  <Card.Root>
    <Card.Header>
      <div class="flex items-center gap-3">
        <Mail class="size-5 text-primary" />
        <Card.Title>Contact Support</Card.Title>
      </div>
    </Card.Header>
    <Card.Content>
      <p class="text-sm">
        If you need help, open a support request or email
        <strong> support@example.com </strong>.
      </p>
      <div class="mt-4 flex gap-2">
        <Button href="mailto:support@example.com" variant="outline" size="sm">
          Email
        </Button>
        <Button variant="ghost" size="sm">
			Live chat
		</Button>
      </div>
    </Card.Content>
  </Card.Root>
</div>
