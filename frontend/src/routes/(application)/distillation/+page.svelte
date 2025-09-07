<script lang="ts">
    import { onMount } from "svelte";
    import { Input } from "$lib/components/ui/input";
    import { Button } from "$lib/components/ui/button";
    import * as Card from "$lib/components/ui/card";
    import Badge from "$lib/components/ui/badge/badge.svelte";
    import { RefreshCw, FileText, CircleX } from "lucide-svelte";
    import { listModels } from "$lib/services/teacher-model";
    import {
        listTasks,
        createTask,
        getTask,
        getTaskLogs,
        cancelTask,
        type DistillTask,
        type TaskLog,
    } from "$lib/services/distillation";
    import * as Select from "$lib/components/ui/select";
    let teacherModels = [] as { id: string; name: string }[];
    let selectedTeacher = "";
    let studentArch = "";
    let params = "";

    let tasks: DistillTask[] = [];
    let polling = false;

    async function fetchModels() {
        try {
            teacherModels = await listModels();
        } catch (err) {
            console.error("failed to list teacher models", err);
        }
    }

    async function fetchTasks() {
        tasks = await listTasks();
    }

    async function handleCreate() {
        try {
            const payload = {
                teacherModelId: selectedTeacher,
                studentArchitecture: studentArch,
                params: params ? JSON.parse(params) : undefined,
            };
            const created = await createTask(payload);
            tasks.unshift(created);
            selectedTeacher = "";
            studentArch = "";
            params = "";
        } catch (err) {
            alert("create failed: " + ((err as any).message || String(err)));
        }
    }

    async function refreshTask(id: string) {
        const t = await getTask(id);
        const idx = tasks.findIndex((x) => x.id === id);
        if (idx >= 0) tasks[idx] = t;
    }

    async function showLogs(id: string) {
        const logs = await getTaskLogs(id);
        const txt = logs
            .map(
                (l) =>
                    `[${l.timestamp}] ${l.level.toUpperCase()}: ${l.message}`,
            )
            .join("\n");
        alert(txt || "no logs");
    }

    async function handleCancel(id: string) {
        if (!confirm("Cancel task?")) return;
        await cancelTask(id);
        await refreshTask(id);
    }

    let pollTimer: any = null;
    function startPolling() {
        if (polling) return;
        polling = true;
        pollTimer = setInterval(async () => {
            try {
                await fetchTasks();
            } catch (e) {
                console.error("poll error", e);
            }
        }, 3000);
    }

    function stopPolling() {
        polling = false;
        if (pollTimer) clearInterval(pollTimer);
        pollTimer = null;
    }

    onMount(async () => {
        await fetchModels();
        await fetchTasks();
        startPolling();
    });
</script>

<div class="p-4 lg:p-6">
    <section>
        <div class="flex items-center justify-between mb-4 mt-6">
            <h2 class="text-lg font-semibold mb-2">Create Distillation Task</h2>
        </div>
        <div class="grid grid-cols-3 gap-2 mb-4">
            <Select.Root
                type="single"
                name="teacher"
                bind:value={selectedTeacher}
            >
                <Select.Trigger class="w-full">
                    {selectedTeacher || "Select Teacher Model"}
                </Select.Trigger>
                <Select.Content>
                    {#each teacherModels as m}
                        <Select.Item value={m.id}>{m.name}</Select.Item>
                    {/each}
                </Select.Content>
            </Select.Root>
            <Input
                placeholder="student architecture (ex: resnet18)"
                bind:value={studentArch}
                class="border p-2"
            />
            <Input
                placeholder="params as JSON e.g. {'epochs'}"
                bind:value={params}
                class="border p-2"
            />
        </div>
        <div class="mb-6">
            <Button
                onclick={handleCreate}
                disabled={!selectedTeacher || !studentArch}>create task</Button
            >
        </div>

        <h2 class="text-lg font-semibold mb-2">Task list</h2>
        <div class="grid gap-3">
            {#if tasks.length === 0}
                <Card.Root>
                    <Card.Content>
                        <div class="flex flex-col items-center justify-center py-8 gap-4">
                            <div class="p-4 rounded-full bg-muted/5">
                                <FileText class="size-8 text-primary" />
                            </div>
                            <div class="text-lg font-medium">No distillation tasks yet</div>
                            <div class="text-sm text-muted-foreground text-center max-w-md">Create your first distillation task by selecting a teacher model, choosing a student architecture, and clicking create. You can also explore demo models to get started.</div>
                            <div class="flex gap-2">
                                <Button onclick={handleCreate} disabled={!selectedTeacher || !studentArch}>Create task</Button>
                                <Button href="/models" variant="ghost">Explore models</Button>
                            </div>
                        </div>
                    </Card.Content>
                </Card.Root>
            {:else}
                {#each tasks as t}
                <Card.Root>
                    <Card.Header>
                        <div class="flex items-center justify-between w-full">
                            <div class="flex items-start gap-3">
                                <div class="space-y-0.5">
                                    <div class="text-sm font-medium">{t.studentArchitecture || 'Unnamed student'}</div>
                                    <div class="text-xs text-muted-foreground">{t.id} • {new Date(t.createdAt).toLocaleString()}</div>
                                </div>
                            </div>
                            <div class="flex items-center gap-3">
                                <Badge class="px-2 py-0.5 text-xs" style="background: {t.status === 'running' ? 'var(--color-primary)/10' : t.status === 'queued' ? 'var(--color-accent)/10' : t.status === 'failed' ? '#fee2e2' : 'var(--color-muted)/8'}">{t.status}</Badge>
                                <div class="flex items-center gap-2">
                                    <Button variant="outline" size="sm" onclick={() => refreshTask(t.id)} title="Refresh"><RefreshCw class="size-4" /></Button>
                                    <Button variant="outline" size="sm" onclick={() => showLogs(t.id)} title="View logs"><FileText class="size-4" /></Button>
                                    {#if t.status === 'queued' || t.status === 'running'}
                                        <Button variant="destructive" size="sm" onclick={() => handleCancel(t.id)} title="Cancel"><CircleX class="size-4" /></Button>
                                    {/if}
                                </div>
                            </div>
                        </div>
                    </Card.Header>
                    <Card.Content>
                        {#if t.params}
                            <div class="text-xs text-muted-foreground mb-2">params: <code class="bg-muted/5 px-1 rounded">{JSON.stringify(t.params)}</code></div>
                        {/if}
                        {#if t.error}
                            <div class="mt-2 text-sm text-red-600">Error: {t.error}</div>
                        {/if}
                        {#if 'progress' in t}
                            <div class="mt-3">
                                <div class="w-full bg-muted/10 h-2 rounded overflow-hidden">
                                    <div class="h-2 bg-primary" style="width: {Math.min(100, (t as any).progress)}%"></div>
                                </div>
                                <div class="text-xs text-muted-foreground mt-1">Progress: {(t as any).progress}%</div>
                            </div>
                        {/if}
                    </Card.Content>
                </Card.Root>
                {/each}
            {/if}
        </div>
    </section>
</div>
