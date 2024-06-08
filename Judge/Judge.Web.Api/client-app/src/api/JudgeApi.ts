import {Api} from "./Api.ts";

export const judgeApi = (): Api<unknown> => {
    const token = localStorage.getItem("token");

    return new Api({
        headers: {"Authorization": `Bearer ${token}`}
    });
}