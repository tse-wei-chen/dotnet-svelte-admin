<script lang="ts">
  import { onMount } from "svelte";
  import {
    listModels,
    uploadModel,
    deleteModel,
  } from "$lib/services/teacher-model";
  import * as Sheet from "$lib/components/ui/sheet/index.js";
  import { Input } from "$lib/components/ui/input";
  import { Button, buttonVariants } from "$lib/components/ui/button";
  import { createSvelteTable, FlexRender } from "$lib/components/ui/data-table";
  import ChevronDownIcon from "@lucide/svelte/icons/chevron-down";
  import * as Table from "$lib/components/ui/table";
  import * as DropdownMenu from "$lib/components/ui/dropdown-menu";
  import * as Select from "$lib/components/ui/select";
  import {
    createColumnHelper,
    getCoreRowModel,
    getSortedRowModel,
    getPaginationRowModel,
    type VisibilityState,
  } from "@tanstack/table-core";
  import type { TeacherModel } from "$lib/interface/teacher-model.interface";

  let name = $state("");
  let framework = $state<"pytorch" | "tensorflow" | "onnx" | "">("");
  let file: File | null = null;
  let uploading = $state(false);
  let uploadError = $state<string | null>(null);
  let models: TeacherModel[] = [];
  let refreshing = $state(false);
  let showDetailsId = $state<string | null>(null);
  let table = $state<any>(null);
  let columnVisibility = $state<VisibilityState>({});

  const columnHelper = createColumnHelper<TeacherModel>();
  const columns = [
    columnHelper.accessor("name", {
      header: "Name",
      cell: (info) => info.getValue(),
    }),
    columnHelper.accessor("filename", {
      header: "File Name",
      cell: (info) => info.getValue(),
    }),
    columnHelper.accessor("framework", {
      header: "Framework",
      cell: (info) => info.getValue(),
    }),
    columnHelper.accessor("size", {
      header: "Size",
      cell: (info) =>
        info.getValue()
          ? Math.round((info.getValue() as number) / 1024) + " KB"
          : "-",
    }),
    columnHelper.accessor("uploadedAt", {
      header: "Uploaded At",
      cell: (info) =>
        info.getValue()
          ? new Date(info.getValue() as string).toLocaleString()
          : "-",
    }),
    columnHelper.accessor("status", {
      header: "Status",
      cell: (info) => info.getValue(),
    }),
  ];

  const allowed = {
    pytorch: [".pt", ".pth", ".bin"],
    tensorflow: [".pb", ".h5", ".keras"],
    onnx: [".onnx"],
  } as const;

  function onFileSelected(e: Event) {
    const input = e.target as HTMLInputElement;
    file = input.files && input.files[0] ? input.files[0] : null;
  }

  async function fetchList() {
    refreshing = true;
    try {
      models = await listModels();
      if (table) {
        table.setOptions((prev: any) => ({ ...(prev as any), data: models }));
      }
    } catch (err) {
      console.error("failed to list models", err);
    } finally {
      refreshing = false;
    }
  }

  onMount(() => {
    table = createSvelteTable<TeacherModel>({
      data: models,
      columns,
      state: {
        get columnVisibility() {
          return columnVisibility;
        },
      },
      onColumnVisibilityChange: (updater) => {
        if (typeof updater === "function")
          columnVisibility = updater(columnVisibility);
        else columnVisibility = updater;
      },
      getCoreRowModel: getCoreRowModel(),
      getSortedRowModel: getSortedRowModel(),
      getPaginationRowModel: getPaginationRowModel(),
      initialState: { pagination: { pageIndex: 0, pageSize: 10 } },
    });

    fetchList();
  });

  function validateSelection() {
    if (!file) return "please select a model file.";
    if (!framework) return "please select a model framework.";
    const ext = "." + (file.name.split(".").pop() ?? "").toLowerCase();
    if (!(allowed[framework] as readonly string[]).includes(ext)) {
      return `unsupported file type: ${ext}`;
    }
    return null;
  }

  async function handleUpload(e?: Event) {
    e?.preventDefault();
    uploadError = null;
    const v = validateSelection();
    if (v) {
      uploadError = v;
      return;
    }
    if (!file) return;

    uploading = true;
    try {
      const fd = new FormData();
      fd.append("file", file);
      fd.append("name", name || file.name);
      fd.append("framework", framework);

      await uploadModel(fd);
      await fetchList();
      name = "";
      framework = "";
      file = null;
      const input = document.getElementById(
        "model-file",
      ) as HTMLInputElement | null;
      if (input) input.value = "";
    } catch (err: any) {
      uploadError = err?.message || String(err);
    } finally {
      uploading = false;
    }
  }

  async function handleDelete(id: string) {
    if (!confirm("Are you sure you want to delete this model?")) return;
    try {
      await deleteModel(id);
      await fetchList();
    } catch (err) {
      console.error(err);
      alert("Delete failed");
    }
  }
</script>

<div class="p-4 lg:p-6">
  <section>
    <div class="flex items-center justify-between mb-4 mt-6">
      <h2 class="text-xl font-semibold mb-2">teacher models</h2>
      <Sheet.Root>
        <Sheet.Trigger class={buttonVariants({ variant: "outline" })}>
          Upload Teacher Model
        </Sheet.Trigger>
        <Sheet.Content side="right">
          <Sheet.Header>
            <Sheet.Title>Upload Teacher Model</Sheet.Title>
            <Sheet.Description>
              Please upload the teacher model here. Click save when done.
            </Sheet.Description>
          </Sheet.Header>
          <div class="grid flex-1 auto-rows-min gap-6 px-4">
            <form onsubmit={handleUpload} class="space-y-3">
              <div class="flex gap-2 items-center">
                <Input
                  class="border p-2 flex-1"
                  placeholder="Model Name (Optional)"
                  bind:value={name}
                />
                <Select.Root
                  type="single"
                  name="framework"
                  bind:value={framework}
                >
                  <Select.Trigger class="border p-2">
                    {framework || "Select Framework"}
                  </Select.Trigger>
                  <Select.Content>
                    <Select.Group>
                      <Select.Label>Framework</Select.Label>
                      <Select.Item value="pytorch" label="PyTorch">
                        PyTorch
                      </Select.Item>
                      <Select.Item value="tensorflow" label="TensorFlow">
                        TensorFlow
                      </Select.Item>
                      <Select.Item value="onnx" label="ONNX">ONNX</Select.Item>
                    </Select.Group>
                  </Select.Content>
                </Select.Root>
              </div>

              <div class="flex gap-2 items-center">
                <Input
                  id="model-file"
                  type="file"
                  onchange={onFileSelected}
                  accept=".pt,.pth,.bin,.onnx,.pb,.h5,.keras"
                />
              </div>
              {#if uploadError}
                <div class="text-red-600">{uploadError}</div>
              {/if}
              <div class="text-sm text-muted-foreground">
                Support PyTorch (.pt/.pth/.bin)、TensorFlow (.pb/.h5/.keras) and
                ONNX (.onnx)
              </div>
            </form>
          </div>
          <Sheet.Footer>
            <Sheet.Close
              class={buttonVariants({ variant: "outline" })}
              type="submit"
              disabled={uploading}
            >
              {#if uploading}
                Uploading...
              {:else}
                Upload
              {/if}
            </Sheet.Close>
          </Sheet.Footer>
        </Sheet.Content>
      </Sheet.Root>
    </div>
    <div>
      {#if table}
        <div class="flex items-center py-4 gap-2">
          <Input
            placeholder="Filter name..."
            value={(table.getColumn("name")?.getFilterValue() as string) ?? ""}
            oninput={(e) =>
              table.getColumn("name")?.setFilterValue(e.currentTarget.value)}
            class="max-w-sm"
          />

          <DropdownMenu.Root>
            <DropdownMenu.Trigger>
              <Button variant="outline" class="ml-auto">
                Columns <ChevronDownIcon class="ml-2" />
              </Button>
            </DropdownMenu.Trigger>
            <DropdownMenu.Content align="end">
              {#each table
                .getAllColumns()
                .filter( (col: { getCanHide: () => any }) => col.getCanHide(), ) as column (column.id)}
                <DropdownMenu.CheckboxItem
                  class="capitalize"
                  checked={column.getIsVisible()}
                  onclick={() =>
                    column.toggleVisibility(!column.getIsVisible())}
                >
                  {column.id}
                </DropdownMenu.CheckboxItem>
              {/each}
            </DropdownMenu.Content>
          </DropdownMenu.Root>

          <Button
            class="btn border px-2 py-1"
            onclick={fetchList}
            disabled={refreshing}
            >{refreshing ? "Refreshing..." : "Refresh"}
          </Button>
        </div>

        <div class="rounded-md border overflow-auto">
          <Table.Root>
            <Table.Header>
              {#each table.getHeaderGroups() as headerGroup (headerGroup.id)}
                {#if headerGroup.headers.some((h: { isPlaceholder: any; column: { getIsVisible: () => any } }) => !h.isPlaceholder && h.column.getIsVisible())}
                  <Table.Row>
                    {#each headerGroup.headers as header (header.id)}
                      {#if !header.isPlaceholder && header.column.getIsVisible()}
                        <Table.Head class="[&:has([role=checkbox])]:pl-3">
                          <FlexRender
                            content={header.column.columnDef.header}
                            context={header.getContext()}
                          />
                        </Table.Head>
                      {/if}
                    {/each}
                    <Table.Head>Actions</Table.Head>
                  </Table.Row>
                {/if}
              {/each}
            </Table.Header>
            <Table.Body>
              {#each table.getRowModel().rows as row (row.id)}
                <Table.Row data-state={row.getIsSelected() && "selected"}>
                  {#each row.getVisibleCells() as cell (cell.id)}
                    <Table.Cell class="[&:has([role=checkbox])]:pl-3">
                      <FlexRender
                        content={cell.column.columnDef.cell}
                        context={cell.getContext()}
                      />
                    </Table.Cell>
                  {/each}
                  <Table.Cell>
                    <div class="flex gap-2">
                      <Button
                        class="px-2 py-1 border"
                        onclick={() =>
                          (showDetailsId =
                            showDetailsId === row.original.id
                              ? null
                              : row.original.id)}>Details</Button
                      >
                      <a
                        class="px-2 py-1 border"
                        href={`/api/teacher/models/${row.original.id}/download`}
                        target="_blank"
                        rel="noreferrer">Download</a
                      >
                      <Button
                        class="px-2 py-1 border text-red-600"
                        onclick={() => handleDelete(row.original.id)}
                      >
                        Delete
                      </Button>
                    </div>
                  </Table.Cell>
                </Table.Row>
                {#if showDetailsId === row.original.id}
                  <Table.Row class="bg-gray-100">
                    <Table.Cell colspan={columns.length + 1}>
                      <div class="space-y-1">
                        <div><strong>Name:</strong> {row.original.name}</div>
                        <div>
                          <strong>File Name:</strong>
                          {row.original.filename}
                        </div>
                        <div>
                          <strong>Framework:</strong>
                          {row.original.framework}
                        </div>
                        <div>
                          <strong>Status:</strong>
                          {row.original.status}
                        </div>
                        <div>
                          <strong>Uploaded At:</strong>
                          {new Date(row.original.uploadedAt).toLocaleString()}
                        </div>
                        <div>
                          <strong>Description:</strong>
                          {row.original.description ?? "-"}
                        </div>
                      </div>
                    </Table.Cell>
                  </Table.Row>
                {/if}
              {:else}
                <Table.Row>
                  <Table.Cell colspan={columns.length} class="h-24 text-center">
                    No data.
                  </Table.Cell>
                </Table.Row>
              {/each}
            </Table.Body>
          </Table.Root>
        </div>

        <div class="flex items-center justify-end space-x-2 pt-4">
          <div class="text-muted-foreground flex-1 text-sm">
            {table.getFilteredSelectedRowModel().rows.length} of {table.getFilteredRowModel()
              .rows.length} row(s) selected.
          </div>
          <div class="space-x-2">
            <Button
              variant="outline"
              size="sm"
              onclick={() => table.previousPage()}
              disabled={!table.getCanPreviousPage()}>Previous</Button
            >
            <Button
              variant="outline"
              size="sm"
              onclick={() => table.nextPage()}
              disabled={!table.getCanNextPage()}>Next</Button
            >
          </div>
        </div>
      {/if}
    </div>
  </section>
</div>
