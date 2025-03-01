import React, {useEffect, useState} from "react";
import {judgeApi} from "../../api/JudgeApi.ts";
import {ColumnType} from "antd/lib/table";
import {EditLanguage, Language} from "../../api/Api.ts";
import {Button, Checkbox, Form, Input, Modal, Table} from "antd";
import {handleError} from "../../helpers/handleError.ts";
import {UserState} from "../../userSlice.ts";
import {useSelector} from "react-redux";
import {useNavigate} from "react-router-dom";
import {CheckOutlined} from "@ant-design/icons";

export const Languages: React.FC = () => {
    const navigate = useNavigate();
    const [isLoading, setLoading] = useState(true);
    const [languages, setLanguages] = useState<Language[]>([]);
    const [editLanguage, setEditLanguage] = useState<EditLanguage>();
    const [isModalOpen, setIsModalOpen] = useState(false);
    const {user}: UserState = useSelector((state: any) => state.user)
    const isAdmin = user?.roles.includes("admin") || false;
    const api = judgeApi().api;

    if (!isAdmin) {
        navigate("/login");
    }

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);

            const result = await api.adminLanguagesList();

            setLanguages(result.data.items);

            setLoading(false);
        }

        fetchData().catch(e => handleError(e));
    }, []);

    const columns: ColumnType<Language>[] = [
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'name'
        },
        {
            title: 'Compilable',
            dataIndex: 'isCompilable',
            key: 'isCompilable',
            align: 'center',
            render: value => value ? <CheckOutlined/> : ''
        },
        {
            title: 'Compiler path',
            dataIndex: 'compilerPath',
            key: 'compilerPath'
        },
        {
            title: 'Compiler options',
            dataIndex: 'compilerOptionsTemplate',
            key: 'compilerOptionsTemplate'
        },
        {
            title: 'Hidden',
            dataIndex: 'isHidden',
            key: 'isHidden',
            align: 'center',
            render: value => value ? <CheckOutlined/> : ''
        }
    ];

    const onSaveModal = async () => {
        if (editLanguage) {
            try {
                await api.adminLanguagesUpdate({...form.getFieldsValue()});

                const result = await api.adminLanguagesList();
                setLanguages(result.data.items);

            } catch (e: any) {
                handleError(e);
            } finally {
                setIsModalOpen(false);
                setEditLanguage(undefined);
            }
        }
    }

    const [form] = Form.useForm();

    return (
        <>
            <Modal title="Edit language"
                   open={isModalOpen}
                   onCancel={() => setIsModalOpen(false)}
                   onOk={onSaveModal}
                   width="60%"
            >
                <Form
                    form={form}
                    layout="horizontal">
                    <Form.Item name="id">
                        <Input type="hidden" value={editLanguage?.id?.toString()}/>
                    </Form.Item>
                    <Form.Item label="Name" name="name">
                        <Input max={128}/>
                    </Form.Item>
                    <Form.Item label="Description" name="description">
                        <Input max={1024}/>
                    </Form.Item>
                    <Form.Item label="Is compilable" name="isCompilable" valuePropName="checked">
                        <Checkbox/>
                    </Form.Item>
                    <Form.Item label="Compiler path" name="compilerPath">
                        <Input max={512}/>
                    </Form.Item>
                    <Form.Item label="Compiler options template" name="compilerOptionsTemplate">
                        <Input max={512}/>
                    </Form.Item>
                    <Form.Item label="Output file template" name="outputFileTemplate">
                        <Input max={512}/>
                    </Form.Item>
                    <Form.Item label="Run string template" name="runStringTemplate">
                        <Input max={512}/>
                    </Form.Item>
                    <Form.Item label="Default file name" name="defaultFileName">
                        <Input max={512}/>
                    </Form.Item>
                    <Form.Item label="AutoDetect file name" name="autoDetectFileName" valuePropName="checked">
                        <Checkbox/>
                    </Form.Item>
                    <Form.Item label="Is hidden" name="isHidden" valuePropName="checked">
                        <Checkbox/>
                    </Form.Item>
                </Form>
            </Modal>
            <Button color="default" type="link" onClick={() => {
                setEditLanguage({} as EditLanguage);
                form.setFieldsValue({} as EditLanguage)
                setIsModalOpen(true);
            }}>Add new language</Button>
            <Table
                style={{cursor: 'pointer'}}
                onRow={(record) => {
                    return {
                        onClick: () => {
                            setEditLanguage(record);
                            form.setFieldsValue(record)
                            setIsModalOpen(true);
                        },
                    };
                }}
                dataSource={languages}
                columns={columns}
                pagination={false}
                loading={isLoading}
            />
        </>
    );
}