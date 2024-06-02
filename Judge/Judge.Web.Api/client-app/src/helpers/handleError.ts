import {AxiosError} from "axios";

export function handleError(e: AxiosError) {
    if (e.response?.status === 404) {
        window.location.replace(`/notFound`);
    }
}