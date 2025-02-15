import {CurrentUser} from "../api/Api.ts";
import {MenuProps} from "antd/lib";
import {Link} from "react-router-dom";

export type MenuItem = Required<MenuProps>['items'][number];

export const buildMenu = (user: CurrentUser | null): MenuItem[] => {
    const subMenu = [];
    if (user?.roles.includes("admin")) {
        const items = {
            label: (<Link to="/administration">Administration</Link>),
            key: 'Administration',
            children: [
                {
                    label: (<Link to="/administration/languages">Languages</Link>),
                    key: 'admin-languages',
                },
                {
                    label: (<Link to="/administration/problems">Problems</Link>),
                    key: 'admin-problems',
                },
                {
                    label: (<Link to="/administration/contests">Contests</Link>),
                    key: 'admin-contests',
                },
                {
                    label: (<Link to="/administration/users">Users</Link>),
                    key: 'admin-users',
                }]
        };

        subMenu.push(items);
    }
    if (user?.login) {
        subMenu.push({label: 'Logout', key: 'Logout', onClick: logout});
    }
    return [
        {
            label: (<Link to="/">Home</Link>),
            key: 'home',
        },
        {
            label: (<Link to="/problems">Problems</Link>),
            key: 'problems',
        },
        {
            label: (<Link to="/contests">Contests</Link>),
            key: 'contests',
        },
        {
            label: (<Link to="/submits">Submits</Link>),
            key: 'submits',
        },
        {
            label: (<Link to="/help">Help</Link>),
            key: 'help'
        },
        {
            label: user?.login || (<Link to="/login">Login</Link>),
            key: 'login',
            children: subMenu,
        }
    ];
}

const logout = () => {
    localStorage.removeItem("token");
    window.location.href = "/login";
}