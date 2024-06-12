import React, {useEffect} from 'react';
import {useDispatch} from 'react-redux';
import {Layout, Menu, theme} from 'antd';
import {Router} from "./Router.tsx";
import {MenuProps} from "antd/lib";
import {judgeApi} from "../api/JudgeApi.ts";
import {setUser} from "../userSlice.ts";

const {Header, Content, Footer} = Layout;

const App: React.FC = () => {
    const {
        token: {colorBgContainer, borderRadiusLG},
    } = theme.useToken();

    const dispatch = useDispatch();

    useEffect(() => {
        const fetchData = async () => {
            const api = judgeApi();
            const response = await api.api.usersMeList();
            const user = response.data;
            
            dispatch(setUser(user));
        };

        fetchData();
    }, []);

    type MenuItem = Required<MenuProps>['items'][number];

    const items: MenuItem[] = [
        {
            label: (<a href="/">Home</a>),
            key: 'home',
        },
        {
            label: (<a href="/problems">Problems</a>),
            key: 'problems',
        },
        {
            label: (<a href="/login">Login</a>),
            key: 'login'
        }
    ];

    return (
        <Layout>
            <Header style={{display: 'flex', alignItems: 'center'}}>
                <div className="demo-logo"/>
                <Menu
                    theme="dark"
                    mode="horizontal"
                    defaultSelectedKeys={['2']}
                    items={items}
                    style={{flex: 1, minWidth: 0}}
                >

                </Menu>
            </Header>
            <Content style={{padding: '0 48px'}}>
                <div
                    style={{
                        background: colorBgContainer,
                        minHeight: 280,
                        padding: 24,
                        borderRadius: borderRadiusLG,
                    }}
                >
                    <Router/>
                </div>
            </Content>
            <Footer style={{textAlign: 'center'}}>
                Judge.NET ©{new Date().getFullYear()}
            </Footer>
        </Layout>
    );
};

export default App;