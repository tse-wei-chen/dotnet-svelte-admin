import type { TeacherModel } from '$lib/interface/teacher-model.interface';
import { apiAuthFetch } from './interceptor';

export async function listModels(): Promise<TeacherModel[]> {
  return await apiAuthFetch('/', 'GET');
}

export async function uploadModel(formData: FormData): Promise<void> {
  await apiAuthFetch('/', 'POST', undefined, formData);
}

export async function deleteModel(id: string): Promise<void> {
  await apiAuthFetch(`/${encodeURIComponent(id)}`, 'DELETE');
}

export async function getModel(id: string): Promise<TeacherModel> {
  return await apiAuthFetch(`/${encodeURIComponent(id)}`, 'GET');
}
