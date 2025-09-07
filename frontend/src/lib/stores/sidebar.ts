import { writable } from 'svelte/store';
import { browser } from '$app/environment';

interface SidebarSettings {
  disabled: boolean;
}

interface SidebarState {
  isOpen: boolean;
  isHover: boolean;
  settings: SidebarSettings;
}

function createSidebarStore() {
  const initialState: SidebarState = {
    isOpen: browser ? localStorage.getItem('sidebar:state') === 'true' : true,
    isHover: false,
    settings: {
      disabled: false
    }
  };

  const { subscribe, set, update } = writable<SidebarState>(initialState);

  return {
    subscribe,
    toggle: () => update(state => {
      const newState = { ...state, isOpen: !state.isOpen };
      if (browser) {
        localStorage.setItem('sidebar:state', newState.isOpen.toString());
      }
      return newState;
    }),
    setOpen: (isOpen: boolean) => update(state => {
      const newState = { ...state, isOpen };
      if (browser) {
        localStorage.setItem('sidebar:state', isOpen.toString());
      }
      return newState;
    }),
    setHover: (isHover: boolean) => update(state => ({ ...state, isHover })),
    getOpenState: (): boolean => {
      let currentState: SidebarState = initialState;
      subscribe(state => currentState = state)();
      return currentState.isOpen || currentState.isHover;
    }
  };
}

export const sidebarStore = createSidebarStore();
