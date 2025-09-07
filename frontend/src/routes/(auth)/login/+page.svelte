<script lang="ts">
    import { goto } from "$app/navigation";
    import UserAuthForm from "$lib/components/auth/user-auth-form.svelte";
    import { onMount } from "svelte";

	onMount(() => {
		if (typeof window !== 'undefined' && window.location.pathname === '/login') {
			if(localStorage.getItem("accessToken")) {
				goto("/home");
			}
		}
	});
</script>

<div
	class="container relative hidden h-[100vh] flex-col items-center justify-center md:grid lg:max-w-none lg:grid-cols-2 lg:px-0"
>
	<div class="bg-muted relative hidden h-full flex-col p-10 text-white lg:flex dark:border-r">
		<div class="absolute inset-0 bg-zinc-900"></div>
		<div class="relative z-20 flex items-center text-lg font-medium">
			<svg
				xmlns="http://www.w3.org/2000/svg"
				viewBox="0 0 24 24"
				fill="none"
				stroke="currentColor"
				stroke-width="2"
				stroke-linecap="round"
				stroke-linejoin="round"
				class="mr-2 h-6 w-6"
			>
				<path d="M15 6v12a3 3 0 1 0 3-3H6a3 3 0 1 0 3 3V6a3 3 0 1 0-3 3h12a3 3 0 1 0-3-3" />
			</svg>
			dotnet svelte Inc.
		</div>
		<div class="relative z-20 mt-auto">
			<blockquote class="space-y-2">
				<p class="text-lg">
					&ldquo;dotnet svelte Inc. aims to improve the experience of building modern web applications.&rdquo;
				</p>
				<footer class="text-sm">wei</footer>
			</blockquote>
		</div>
	</div>
	<div class="lg:p-8">
		<div class="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
			<div class="flex flex-col space-y-2 text-center">
				<h1 class="text-2xl font-semibold tracking-tight">login</h1>
			</div>
			<UserAuthForm />
			<p class="text-muted-foreground px-8 text-center text-sm">
				By clicking continue, you agree to our
				<a href="/terms" class="hover:text-primary underline underline-offset-4">
					Terms of Service
				</a>
				and
				<a href="/privacy" class="hover:text-primary underline underline-offset-4">
					Privacy Policy
				</a>.
			</p>
		</div>
	</div>
</div>