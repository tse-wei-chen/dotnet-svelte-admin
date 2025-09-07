import { apiFetch } from "./interceptor";

export const register = async (username: string, email: string, password: string) => {
    return await apiFetch(
        '/api/auth/register',
        'POST',
        undefined,
        { username, email, password }
    );
};

export const login = async (email: string, password: string) => {
    return await apiFetch(
        '/api/auth/login',
        'POST',
        undefined,
        { email, password }
    );
}

export const logout = async () => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('userinfo');
    window.location.replace('/login');
}

export const refreshToken = async () => {
    return await apiFetch(
        '/api/auth/refresh-token',
        'POST',
        undefined,
        { refreshToken: localStorage.getItem('refreshToken') }
    );
}