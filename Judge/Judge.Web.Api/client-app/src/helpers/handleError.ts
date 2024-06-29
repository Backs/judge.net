import {AxiosError} from "axios";
import {useDispatch} from "react-redux";
import {removeUser} from "../userSlice.ts";

export function handleError(e: AxiosError) {
    if (e.response?.status === 404) {
        window.location.replace(`/notFound`);
    } else if (e.response?.status === 401 || e.response?.status === 403) {
        const dispatch = useDispatch();
        localStorage.removeItem("token");
        dispatch(removeUser());
        window.location.replace(`/login`);
    }
}