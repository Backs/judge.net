import {Api} from "./Api.ts";

export const judgeApi = (token?: string): Api<unknown> => {
    const bearer = token || localStorage.getItem("token");

    return new Api({
        headers: {"Authorization": `Bearer ${bearer}`}
    });
}