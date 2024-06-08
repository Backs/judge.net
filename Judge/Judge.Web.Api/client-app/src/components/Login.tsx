import React from "react";
import {LockOutlined, MailOutlined} from '@ant-design/icons';
import {Button, Form, Input} from "antd";
import {judgeApi} from "../api/JudgeApi.ts";
import {useNavigate} from "react-router-dom";

export const Login: React.FC = () => {
    const navigate = useNavigate();
    const onFinish = async (values: any) => {
        const api = judgeApi();
        const token = await api.api.loginTokenCreate({email: values.email, password: values.password});
        localStorage.setItem("token", token.data.token);

        navigate("/problems");
        
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