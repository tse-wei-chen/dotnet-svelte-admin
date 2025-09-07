import { apiAuthFetch } from './interceptor';

export type DistillTask = {
  id: string;
  teacherModelId: string;
  studentArchitecture: string;
  params?: Record<string, any>;
  status: 'queued' | 'running' | 'completed' | 'failed';
  createdAt: string;
  updatedAt?: string;
  error?: string | null;
};

export type TaskLog = {
  timestamp: string;
  level: 'info' | 'warn' | 'error';
  message: string;
};

const BASE = '/api/distillation/tasks';

export async function listTasks(): Promise<DistillTask[]> {
  return await apiAuthFetch('/', 'GET');
}

export async function createTask(payload: {
  teacherModelId: string;
  studentArchitecture: string;
  params?: Record<string, any>;
}): Promise<DistillTask> {
  return await apiAuthFetch('/', 'POST', undefined, payload);
}

export async function getTask(id: string): Promise<DistillTask> {
  return await apiAuthFetch(`/${encodeURIComponent(id)}`, 'GET');
}

export async function getTaskLogs(id: string): Promise<TaskLog[]> {
  return await apiAuthFetch(`/${encodeURIComponent(id)}/logs`, 'GET');
}

export async function cancelTask(id: string): Promise<void> {
  await apiAuthFetch(`/${encodeURIComponent(id)}/cancel`, 'POST');
}
