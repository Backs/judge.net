import React from "react";
import {LockOutlined, MailOutlined} from '@ant-design/icons';
import {Button, Form, Input} from "antd";
import {Api} from "../api/Api.ts";

export const Login: React.FC = () => {

    const onFinish = async (values: any) => {
        const api = new Api();
        const token = await api.api.loginTokenCreate({email: values.email, password: values.password});
        console.log('Received values of form: ', token.data);
    };
    return (
        <Form
            name="normal_login"
            className="login-form"
            initialValues={{remember: true}}
            onFinish={onFinish}
        >
            <Form.Item
                name="email"
                rules={[{required: true, message: 'Please input your Email!'}]}
            >
                <Input prefix={<MailOutlined className="site-form-item-icon"/>} placeholder="Email"/>
            </Form.Item>
            <Form.Item
                name="password"
                rules={[{required: true, message: 'Please input your Password!'}]}
            >
                <Input
                    prefix={<LockOutlined className="site-form-item-icon"/>}
                    type="password"
                    placeholder="Password"
                />
            </Form.Item>
            <Form.Item>
                <Button type="primary" htmlType="submit" className="login-form-button">
                    Log in
                </Button>
            </Form.Item>
        </Form>
    );
};