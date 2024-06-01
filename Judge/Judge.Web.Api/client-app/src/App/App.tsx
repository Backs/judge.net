import React from 'react';
import {Layout, Menu, theme} from 'antd';
import {Router} from "./Router.tsx";
import {MenuProps} from "antd/lib";

const {Header, Content, Footer} = Layout;

const App: React.FC = () => {
    const {
        token: {colorBgContainer, borderRadiusLG},
    } = theme.useToken();

    type MenuItem = Required<MenuProps>['items'][number];

    const items: MenuItem[] = [
        {
            label: (<a href="/">Home</a>),
            key: 'home',
        },
        {
            label: (<a href="/problems">Problems</a>),
            key: 'problems',
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