import React, {useState} from "react";
import {LockOutlined, MailOutlined} from '@ant-design/icons';
import {Button, Col, Form, Input, Row} from "antd";
import {judgeApi} from "../api/JudgeApi.ts";
import {Link, useNavigate} from "react-router-dom";
import {handleError} from "../helpers/handleError.ts";
import {Typography} from 'antd';
import {setUser} from "../userSlice.ts";
import {useDispatch} from "react-redux";

const {Text} = Typography;

export const Login: React.FC = () => {
    const navigate = useNavigate();
    const [isLoading, setLoading] = useState<boolean>();
    const [errors, setErrors] = useState<React.ReactNode[]>();
    const dispatch = useDispatch();

    const onFinish = async (values: any) => {
        setLoading(true);
        let api = judgeApi();
        
        try {
            const token = await api.api.loginTokenCreate({email: values.email, password: values.password});
            localStorage.setItem("token", token.data.token);

            api = judgeApi(token.data.token);
            const response = await api.api.usersMeList();
            const user = response.data;

            dispatch(setUser(user));

            navigate("/problems");
        } catch (e: any) {
            setLoading(false);

            const status = e.response.status;

            if (status === 404) {
                setErrors([<Text type={"danger"}>User not found</Text>]);
            } else if (status === 400) {
                setErrors([<Text type={"danger"}>Invalid password</Text>]);
            } else {
                handleError(e);
            }
        }
    };
    return (
        <Row justify="center" align="middle">
            <Col span={6}>
                <Form
                    className="login-form"
                    initialValues={{remember: true}}
                    onFinish={onFinish}
                    layout="vertical"
                    autoComplete="on"
                    preserve={true}
                >
                    <Form.Item
                        name="email"
                        rules={[{required: true, message: 'Please input your Email!'}]}
                    >
                        <Input prefix={<MailOutlined className="site-form-item-icon"/>} placeholder="Email"
                               autoComplete="email"/>
                    </Form.Item>
                    <Form.Item
                        name="password"
                        rules={[{required: true, message: 'Please input your Password!'}]}
                    >
                        <Input.Password
                            prefix={<LockOutlined className="site-form-item-icon"/>}
                            type="password"
                            placeholder="Password"
                            autoComplete="current-password"
                        />
                    </Form.Item>
                    <Form.Item>
                        <Button type="primary" loading={isLoading} htmlType="submit" className="login-form-button">
                            Log in
                        </Button>
                    </Form.Item>
                    Or <Link to="/register">register now!</Link>
                    <Form.ErrorList errors={errors}/>
                </Form>
            </Col>
        </Row>
    );
};