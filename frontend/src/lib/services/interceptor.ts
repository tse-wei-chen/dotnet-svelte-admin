import { PUBLIC_apiUrl } from "$env/static/public";
import { logout, refreshToken } from "./auth-service";

export async function apiAuthFetch(url: string, method: 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH' | 'OPTIONS', headers?: HeadersInit, body?: any, retry = true) {
    let headersObj: Record<string, string> = {};
    if (headers instanceof Headers) {
        headers.forEach((value, key) => {
            headersObj[key] = value;
        });
    } else if (Array.isArray(headers)) {
        headers.forEach(([key, value]) => {
            headersObj[key] = value;
        });
    } else if (typeof headers === 'object' && headers) {
        headersObj = { ...headers } as Record<string, string>;
    }
    // read access token (may be stored as plain string or JSON string)
    let raw = localStorage.getItem('accessToken');
    let token = raw;
    try {
        if (raw && raw.startsWith('"') && raw.endsWith('"')) token = JSON.parse(raw);
    } catch { }
    if (token) headersObj['Authorization'] = 'Bearer ' + token;

    const fetchOptions: RequestInit = {
        method,
        headers: headersObj,
    };
    if (body) {
        if (body instanceof FormData) {
            fetchOptions.body = body;
            fetchOptions.headers = headersObj;
        } else {
            fetchOptions.body = typeof body === 'string' ? body : JSON.stringify(body);
            if (!('content-type' in Object.keys(headersObj).reduce((acc, k) => { acc[k.toLowerCase()] = true; return acc; }, {} as Record<string, boolean>))) {
                headersObj['Content-Type'] = 'application/json';
                fetchOptions.headers = headersObj;
            }
        }
    }

    try {
        const response = await fetch(PUBLIC_apiUrl + url, fetchOptions);
        if (response.status === 401 && retry) {
            const refreshTokened = await refreshToken();
            let headers: HeadersInit = {};
            // assume refreshToken() will update localStorage; retry with new access token
            let newRaw = localStorage.getItem('accessToken');
            if (newRaw) {
                try { if (newRaw.startsWith('"') && newRaw.endsWith('"')) newRaw = JSON.parse(newRaw); } catch { }
                headers['Authorization'] = 'Bearer ' + newRaw;
            }
            return apiFetch(url, method, headers, body, true);
        }
        if (response.status === 403) {
            logout();
            throw new Error('Forbidden: localStorage cleared');
        }
        if (!response.ok) {
            let error;
            try {
                error = await response.json();
            } catch {
                error = { message: 'Fetch error' };
            }
            throw new Error(error.message || 'Fetch error');
        }
        return response.json();
    } catch (err) {
        console.error('API Error:', err);
        throw err;
    }
}

export async function apiFetch(url: string, method: 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH' | 'OPTIONS', headers?: HeadersInit, body?: any, retry = true) {
    let headersObj: Record<string, string> = {};
    if (headers instanceof Headers) {
        headers.forEach((value, key) => {
            headersObj[key] = value;
        });
    } else if (Array.isArray(headers)) {
        headers.forEach(([key, value]) => {
            headersObj[key] = value;
        });
    } else if (typeof headers === 'object' && headers) {
        headersObj = { ...headers } as Record<string, string>;
    }

    const fetchOptions: RequestInit = {
        method,
        headers: headersObj,
    };
    if (body) {
        fetchOptions.body = typeof body === 'string' ? body : JSON.stringify(body);
        if (!('content-type' in Object.keys(headersObj).reduce((acc, k) => { acc[k.toLowerCase()] = true; return acc; }, {} as Record<string, boolean>))) {
            headersObj['Content-Type'] = 'application/json';
            fetchOptions.headers = headersObj;
        }
    }

    try {
        const response = await fetch(PUBLIC_apiUrl + url, fetchOptions);
        if (!response.ok) {
            let error;
            try {
                error = await response.json();
            } catch {
                error = { message: 'Fetch error' };
            }
            throw new Error(error.message || 'Fetch error');
        }
        return response.json();
    } catch (err) {
        console.error('API Error:', err);
        throw err;
    }
}