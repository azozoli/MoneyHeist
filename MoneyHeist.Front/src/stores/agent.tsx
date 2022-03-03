import axios, { AxiosResponse } from 'axios';
import frontendSettings from '../helper/frontendSettings';
import { history } from '../helper/configure/history';

axios.defaults.baseURL = frontendSettings.ApiUrl;

const responseBody = (response: AxiosResponse) => response.data;

/* axios.interceptors.request.use((config, store) => {
    const state = store.getState();
    config.headers.Language = 'en';

    return config;
}); */

axios.interceptors.request.use(
    (config) => {
        const token = window.localStorage.getItem('jwt');
        if (token) config.headers.Authorization = `Bearer ${token}`;
        return config;
    },
    (error) => {
        if (error.response.status === 401) {
            window.localStorage.removeItem('jwt');
            history.push('/test');
        }
        return error;
    },
);

/* axios.interceptors.response.use(async (response) => {
    const totalPages = response.headers['totalpages'];
    if (totalPages) {
        response.data = new PaginatedResult(response.data, totalPages);
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        return response as AxiosResponse<PaginatedResult<any>>;
    }

    return response;
}); */

export const requests = {
    // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
    get: (url: string) => axios.get(url).then(responseBody),
    // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
    // eslint-disable-next-line @typescript-eslint/ban-types
    post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
    // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
    // eslint-disable-next-line @typescript-eslint/ban-types
    put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
    // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
    // eslint-disable-next-line @typescript-eslint/ban-types
    delete: (url: string, body: {}) => axios.delete(url, body).then(responseBody),
};
