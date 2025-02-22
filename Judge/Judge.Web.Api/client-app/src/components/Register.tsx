﻿import React, {ReactNode, useState} from "react";
import {Button, Col, Form, Input, Row} from "antd";
import {judgeApi} from "../api/JudgeApi.ts";
import {useNavigate} from "react-router-dom";
import {LockOutlined, MailOutlined, UserOutlined} from "@ant-design/icons";
import { Typography } from 'antd';
const { Text } = Typography;


export const Register: React.FC = () => {
    const [form] = Form.useForm();
    const [errors, setErrors] = useState<ReactNode[]>([]);
    const navigate = useNavigate();
    const onFinish = async (values: any) => {
        console.log('Received values of form: ', values);

        const api = judgeApi();
        try {
            await api.api.usersUpdate({
                email: values.email,
                password: values.password,
                login: values.login
            });

            navigate("/login");
        } catch (e: any) {
            if (e.response?.status === 409) {
                setErrors([<Text type="danger">User with this login or email already exists.</Text>]);
            }
            console.log(e);
        }
    };

    return (
        <Row justify="center" align="middle">
            <Col span={8}>
                <Form
                    form={form}
                    name="register"
                    onFinish={onFinish}
                    scrollToFirstError
                >
                    <Form.Item
                        name="email"
                        rules={[
                            {
                                type: 'email',
                                message: 'The input is not valid E-mail!',
                            },
                            {
                                required: true,
                                message: 'Please input your E-mail!',
                            },
                        ]}
                    >
                        <Input prefix={<MailOutlined className="site-form-item-icon"/>} placeholder="Email"
                               autoComplete="email"/>
                    </Form.Item>

                    <Form.Item
                        name="login"
                        rules={[
                            {
                                required: true,
                                message: 'Please input your Login!',
                            },
                        ]}
                    >
                        <Input prefix={<UserOutlined className="site-form-item-icon"/>} placeholder="User name"
                               autoComplete="login"/>
                    </Form.Item>

                    <Form.Item
                        name="password"
                        rules={[
                            {
                                required: true,
                                message: 'Please input your password!',
                                min: 6
                            },
                        ]}
                        hasFeedback
                    >
                        <Input.Password prefix={<LockOutlined className="site-form-item-icon"/>} placeholder="Password"
                                        autoComplete="new-password"/>
                    </Form.Item>

                    <Form.Item
                        name="confirm"
                        dependencies={['password']}
                        hasFeedback
                        rules={[
                            {
                                required: true,
                                message: 'Please confirm your password!',
                            },
                            ({getFieldValue}) => ({
                                validator(_, value) {
                                    if (!value || getFieldValue('password') === value) {
                                        return Promise.resolve();
                                    }
                                    return Promise.reject(new Error('The new password that you entered do not match!'));
                                },
                            }),
                        ]}
                    >
                        <Input.Password prefix={<LockOutlined className="site-form-item-icon"/>}
                                        placeholder="Confirm password" autoComplete="new-password"/>
                    </Form.Item>
                    <Form.Item>
                        <Form.ErrorList errors={errors}/>
                    </Form.Item>
                    <Form.Item>
                        <Button type="primary" htmlType="submit">
                            Register
                        </Button>
                    </Form.Item>
                </Form>
            </Col>
        </Row>

    )
};