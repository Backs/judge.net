import {AxiosError} from "axios";

export function handleError(e: AxiosError) {
    if (e.response?.status === 404) {
        window.location.replace(`/notFound`);
    } else if (e.response?.status === 401 || e.response?.status === 403) {
        localStorage.removeItem("token");
        window.location.href = `/login`;
    }
}