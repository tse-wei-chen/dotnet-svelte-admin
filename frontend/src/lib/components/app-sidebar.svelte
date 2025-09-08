<script lang="ts">
  import CameraIcon from "@tabler/icons-svelte/icons/camera";
  import ChartBarIcon from "@tabler/icons-svelte/icons/chart-bar";
  import FileAiIcon from "@tabler/icons-svelte/icons/file-ai";
  import FileDescriptionIcon from "@tabler/icons-svelte/icons/file-description";
  import FolderIcon from "@tabler/icons-svelte/icons/folder";
  import HelpIcon from "@tabler/icons-svelte/icons/help";
  import InnerShadowTopIcon from "@tabler/icons-svelte/icons/inner-shadow-top";
  import SearchIcon from "@tabler/icons-svelte/icons/search";
  import SettingsIcon from "@tabler/icons-svelte/icons/settings";
  import UsersIcon from "@tabler/icons-svelte/icons/users";
  import NavMain from "./nav-main.svelte";
  import NavSecondary from "./nav-secondary.svelte";
  import NavUser from "./nav-user.svelte";
  import * as Sidebar from "$lib/components/ui/sidebar/index.js";
  import { onMount, type ComponentProps } from "svelte";
  import HomeIcon from "@tabler/icons-svelte/icons/home";
  import CpuIcon from "@tabler/icons-svelte/icons/cpu";
  import UserIcon from "@tabler/icons-svelte/icons/user";
  import { goto } from "$app/navigation";
  let data = $state({
    user: {
      name: "shadcn",
      email: "m@example.com",
      avatar: ""
    },
    navMain: [
      {
        title: "Home",
        url: "/home",
        icon: HomeIcon
      },
      {
        title: "Teacher",
        url: "/teacher",
        icon: UserIcon
      },
      {
        title: "Distillation",
        url: "/distillation",
        icon: CpuIcon
      },
      {
        title: "Analytics",
        url: "/analytics",
        icon: ChartBarIcon
      },
      {
        title: "Activity",
        url: "/activity",
        icon: UsersIcon
      },
      {
        title: "Reports",
        url: "/reports",
        icon: FileDescriptionIcon
      }
    ],
    navSecondary: [
      {
        title: "Settings",
        url: "/settings",
        icon: SettingsIcon
      },
      {
        title: "Get Help",
        url: "/help",
        icon: HelpIcon
      },
      {
        title: "Search",
        url: "/search",
        icon: SearchIcon
      }
    ]
  });

  onMount(() => {
    let userinfo = localStorage.getItem("userinfo");
    let accessToken = localStorage.getItem("accessToken");

    if (userinfo && accessToken) {
      data.user = JSON.parse(userinfo);
    } else {
      goto("/login");
    }
  });

  let { ...restProps }: ComponentProps<typeof Sidebar.Root> = $props();
</script>

<Sidebar.Root collapsible="offcanvas" {...restProps}>
  <Sidebar.Header>
    <Sidebar.Menu>
      <Sidebar.MenuItem>
        <Sidebar.MenuButton class="data-[slot=sidebar-menu-button]:!p-1.5">
          {#snippet child({ props })}
            <a href="/home" {...props}>
              <InnerShadowTopIcon class="!size-5" />
              <span class="text-base font-semibold"> dotnet svelte Inc. </span>
            </a>
          {/snippet}
        </Sidebar.MenuButton>
      </Sidebar.MenuItem>
    </Sidebar.Menu>
  </Sidebar.Header>
  <Sidebar.Content>
    <NavMain items={data.navMain} />
    <NavSecondary items={data.navSecondary} class="mt-auto" />
  </Sidebar.Content>
  <Sidebar.Footer>
    <NavUser user={data.user} />
  </Sidebar.Footer>
</Sidebar.Root>
