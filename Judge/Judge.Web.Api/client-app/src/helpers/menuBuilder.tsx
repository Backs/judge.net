import {CurrentUser} from "../api/Api.ts";
import {MenuProps} from "antd/lib";

export type MenuItem = Required<MenuProps>['items'][number];

export const buildMenu = (user: CurrentUser | null): MenuItem[] => {
    const subMenu = [];
    if (user?.roles.includes("admin")) {
        const items = {
            label: (<a href="/administration">Administration</a>),
            key: 'Administration',
            children: [{
                label: (<a href="/administration/languages">Languages</a>),
                key: 'languages',
            }]
        };

        subMenu.push(items);
    }
    if (user?.login) {
        subMenu.push({label: 'Logout', key: 'Logout', onClick: logout});
    }
    return [
        {
            label: (<a href="/">Home</a>),
            key: 'home',
        },
        {
            label: (<a href="/problems">Problems</a>),
            key: 'problems',
        },
        {
            label: (<a href="/contests">Contests</a>),
            key: 'contests',
        },
        {
            label: (<a href="/submits">Submits</a>),
            key: 'submits',
        },
        {
            label: user?.login || (<a href="/login">Login</a>),
            key: 'login',
            children: subMenu,
        }
    ];
}

const logout = () => {
    localStorage.removeItem("token");
    window.location.href = "/login";
}