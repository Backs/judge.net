import React, {useEffect} from 'react';
import {Layout, Menu, theme} from 'antd';
import {Router} from "./Router.tsx";
import {useDispatch, useSelector} from "react-redux";
import {setUser, UserState} from "../userSlice.ts";
import {judgeApi} from "../api/JudgeApi.ts";
import {buildMenu} from "../helpers/menuBuilder.tsx";

const {Header, Content, Footer} = Layout;

const App: React.FC = () => {
    const {
        token: {colorBgContainer, borderRadiusLG},
    } = theme.useToken();

    const dispatch = useDispatch();
    const {user}: UserState = useSelector((state: any) => state.user);

    useEffect(() => {
        const fetchData = async () => {
            const api = judgeApi();
            const response = await api.api.usersMeList();
            const user = response.data;

            dispatch(setUser(user));
        };

        fetchData().catch(_ => {
        });
    }, []);

    const items = buildMenu(user);

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