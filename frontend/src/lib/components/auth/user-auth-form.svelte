<script lang="ts" module>
	import { z } from "zod";
	export const formSchema = z.object({
		email: z.string().email({ message: "請輸入有效的 Email" }),
		password: z.string().min(8, { message: "密碼長度必須至少 8 個字符" }).max(100, { message: "密碼長度不能超過 100 個字符" }),
	});
</script>

<script lang="ts">
	import type { UserInfo } from '$lib/interface/user-info.interface';
	import { superForm } from "sveltekit-superforms";
	import { zodClient } from "sveltekit-superforms/adapters";
	import { cn } from "$lib/utils.js";
	import type { HTMLAttributes } from "svelte/elements";
	import { LoaderCircle } from "lucide-svelte";
	import Input from "../ui/input/input.svelte";
	import Button from "../ui/button/button.svelte";
	import * as Form from "../ui/form/index";
    import { login } from "$lib/services/auth-service";
    import { goto } from "$app/navigation";
	let { class: className, ...restProps }: HTMLAttributes<HTMLElement> =
		$props();

	let isLoading = $state(false);
	const form = superForm(
		{ email: "", password: "" },
		{
			validators: zodClient(formSchema as any),
			SPA: true,
			onUpdate: async ({ form: f }) => {
				if (f.valid) {
					isLoading = true;
					try {
						const response = await login(f.data.email, f.data.password);
						if (response && response.data) {
							localStorage.setItem("accessToken", response.data?.accessToken);
							localStorage.setItem("refreshToken", response.data?.refreshToken);
							const userinfo: UserInfo = {
								name: response.data?.userName,
								email: response.data?.email,
								avatar: response.data?.avatar
							}
							localStorage.setItem("userinfo", JSON.stringify(userinfo));
						}
					} catch (err) {
						console.error('login failed', err);
						alert('Login failed: ' + (err as any).message || String(err));
					} finally {
						isLoading = false;
						await goto('/home');
					}
				} else {
					return;
				}
			},
		}
	);

	const { form: formData, enhance } = form;
</script>

<div class={cn("grid gap-6", className)} {...restProps}>
	<form method="post" use:enhance>
		<div class="grid gap-2">
			<div class="grid gap-1">
				<Form.Field {form} name="email">
					<Form.Control>
						{#snippet children({ props }: { props: any })}
							<Form.Label class="sr-only" for="email">
								Email
							</Form.Label>
							<Input
								id="email"
								placeholder="name@example.com"
								type="email"
								autocapitalize="none"
								autocomplete="email"
								autocorrect="off"
								disabled={isLoading}
								{...props}
								bind:value={$formData.email}
							/>
						{/snippet}
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>
				<Form.Field {form} name="password">
					<Form.Control>
						{#snippet children({ props }: { props: any })}
							<Form.Label class="sr-only" for="password">
								Password
							</Form.Label>
							<Input
								id="password"
								placeholder="••••••••"
								type="password"
								autocapitalize="none"
								autocomplete="current-password"
								autocorrect="off"
								disabled={isLoading}
								{...props}
								bind:value={$formData.password}
							/>
						{/snippet}
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>
			</div>
			<Button disabled={isLoading} type="submit" class="w-full">
				{#if isLoading}
					<LoaderCircle class="mr-2 size-4 animate-spin" />
				{/if}
				Sign In
			</Button>
		</div>
	</form>
	<div class="relative">
		<div class="absolute inset-0 flex items-center">
			<span class="w-full border-t"></span>
		</div>
	</div>
</div>