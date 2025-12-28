import {useNavigate, useParams} from "react-router-dom";
import {judgeApi} from "../../api/JudgeApi.ts";
import React, {useEffect, useState} from "react";
import {Button, Modal, DatePicker, Form, Input, Select, Space, Spin, Switch, Flex, Row, Col} from "antd";
import {handleError} from "../../helpers/handleError.ts";
import {ContestRules, EditContest, EditContestProblem, ProblemInfo} from "../../api/Api.ts";
import dayjs from 'dayjs';
import utc from 'dayjs/plugin/utc';
import {UserState} from "../../userSlice.ts";
import {useSelector} from "react-redux";
import {EditOutlined, MinusCircleOutlined, PlusOutlined} from '@ant-design/icons';

export const ContestEdit: React.FC = () => {
    dayjs.extend(utc);

    const navigate = useNavigate();
    const {contestId} = useParams();
    const [isLoading, setLoading] = useState(true);
    const [isModalOpen, setModalOpen] = useState(false);
    const [editProblem, setEditProblem] = useState<EditContestProblem>();
    const [editIndex, setEditIndex] = useState(-1);
    const [contest, setContest] = useState<EditContest>({
        isOpened: false,
        checkPointTime: null,
        name: "",
        id: null,
        oneLanguagePerTask: false,
        rules: ContestRules.Acm,
        problems: [],
        startTime: "",
        finishTime: ""
    })

    const [data, setData] = useState<ProblemInfo[]>([]);

    const api = judgeApi();
    const [form] = Form.useForm();

    const {user}: UserState = useSelector((state: any) => state.user)
    const isAdmin = user?.roles.includes("admin") || false;

    if (!isAdmin) {
        navigate("/login");
    }

    useEffect(() => {
        const fetchData = async () => {
            if (contestId) {
                const response = await api.api.contestsEditableDetail(Number(contestId));
                const serverData = response.data;

                setContest(serverData);

                const toLocalFaceValue = (utcStr?: string | null) => {
                    if (!utcStr) return null;
                    return dayjs(dayjs.utc(utcStr).format('YYYY-MM-DD HH:mm:ss'));
                };

                const formValues = {
                    ...serverData,
                    startTime: toLocalFaceValue(serverData.startTime),
                    finishTime: toLocalFaceValue(serverData.finishTime),
                    checkPointTime: toLocalFaceValue(serverData.checkPointTime),
                };

                form.setFieldsValue(formValues);
            } else {
                form.setFieldsValue(contest);
            }
            setLoading(false);
        }

        fetchData().catch(e => handleError(e));
    }, [contestId, form]);

    const onFinish = async (values: any) => {
        setLoading(true);

        const toUtcSubmitValue = (localDayjs: any) => {
            if (!localDayjs) return null; // или "" в зависимости от требований API
            return dayjs.utc(localDayjs.format('YYYY-MM-DD HH:mm:ss')).format();
        };

        const submitValues: EditContest = {
            ...values,
            startTime: toUtcSubmitValue(values.startTime),
            finishTime: toUtcSubmitValue(values.finishTime),
            checkPointTime: toUtcSubmitValue(values.checkPointTime),
        };

        setContest(submitValues);

        try {
            const response = await api.api.contestsUpdate(submitValues);
            navigate(`/contests/${response.data.id}/edit`, {replace: true});
        } catch (e: any) {
            handleError(e)
        } finally {
            setLoading(false);
        }
    }

    const onEditProblem = (index: number) => {
        setEditIndex(index);
        const problems = form.getFieldValue('problems');
        setEditProblem(problems[index]);
        setModalOpen(true);
    }


    const onSaveModal = () => {
        if (editProblem) {
            const problems = form.getFieldValue('problems');
            problems[editIndex] = editProblem;
            form.setFieldValue('problems', problems);

            setContest({...contest, problems});
        }
        setModalOpen(false);
        setData([]);
    }

    const handleSearch = async (value: string) => {
        if (value && value.length >= 3) {
            const response = await api.api.problemsList({Name: value, Skip: 0, Take: 10});
            setData(response.data.items);
        }
    };

    const onChangeEditProblem = (value: number, option: any) => {
        setEditProblem({
            problemId: value,
            name: option.label,
            label: editProblem!.label
        });
    };

    const onChangeLabel = (value: string) => {
        setEditProblem({
            problemId: editProblem!.problemId,
            name: editProblem!.name,
            label: value
        });
    };

    const onCloseModal = () => {
        setModalOpen(false);
        setData([]);
    };

    return (isLoading ? <Spin size="large"/> :
        <Row>
            <Col span={12}>
                <Modal title="Edit problem"
                       open={isModalOpen}
                       onCancel={onCloseModal}
                       onOk={onSaveModal}
                >
                    <Flex vertical gap={20}>
                        <Input maxLength={10}
                               value={editProblem?.label}
                               onChange={e => onChangeLabel(e.target.value)}
                        />
                        <Select
                            showSearch
                            value={editProblem?.problemId}
                            labelRender={_ => editProblem?.name}
                            filterOption={false}
                            onSearch={handleSearch}
                            onChange={(value: number, option) => onChangeEditProblem(value, option)}
                            options={data.map((d) => ({
                                value: d.id,
                                label: d.name,
                            }))}
                        />
                    </Flex>
                </Modal>
                <Form
                    form={form}
                    layout="horizontal"
                    onFinish={onFinish}
                >
                    <Form.Item name="id">
                        <Input type="hidden" value={contest.id?.toString()}/>
                    </Form.Item>
                    <Form.Item label="Name" name="name">
                        <Input max={200}/>
                    </Form.Item>
                    <Form.Item label="Rules" name="rules">
                        <Select>
                            <Select.Option value="Acm">Acm</Select.Option>
                            <Select.Option value="Points">Points</Select.Option>
                            <Select.Option value="CheckPoint">CheckPoint</Select.Option>
                            <Select.Option value="Dynamic">Dynamic</Select.Option>
                        </Select>
                    </Form.Item>
                    <Form.Item label="Start time" name="startTime">
                        <DatePicker allowClear={false} showTime/>
                    </Form.Item>
                    <Form.Item label="Finish time" name="finishTime">
                        <DatePicker allowClear={false} showTime/>
                    </Form.Item>
                    {contest.rules === ContestRules.CheckPoint &&
                        <Form.Item label="Check point time" name="checkPointTime">
                            <DatePicker allowClear={false} showTime/>
                        </Form.Item>}
                    <Form.Item label="One language per task" name="oneLanguagePerTask">
                        <Switch/>
                    </Form.Item>
                    <Form.Item label="Is opened" name="isOpened">
                        <Switch/>
                    </Form.Item>
                    <Form.List name="problems">
                        {(fields, {add, remove}) => (
                            <>
                                {fields.map(({key, name, ...restField}) => (
                                    <Space key={key} style={{display: 'flex', marginBottom: 8}} align="baseline">
                                        <Form.Item
                                            {...restField}
                                            required={true}
                                            name={[name, 'label']}
                                        >
                                            <Input disabled={true}/>
                                        </Form.Item>
                                        <Form.Item
                                            {...restField}
                                            required={true}
                                            name={[name, 'name']}
                                        >
                                            <Input disabled={true} style={{width: 200}}/>
                                        </Form.Item>
                                        <EditOutlined onClick={() => onEditProblem(name)}/>
                                        <MinusCircleOutlined onClick={() => remove(name)}/>
                                    </Space>
                                ))}
                                <Form.Item>
                                    <Button type="dashed" onClick={() => {
                                        add({
                                            name: "",
                                            problemId: 0,
                                            label: ""
                                        });
                                    }} block icon={<PlusOutlined/>}>
                                        Add problem
                                    </Button>
                                </Form.Item>
                            </>

                        )}
                    </Form.List>
                    <Form.Item>
                        <Button type="primary" htmlType="submit">Save</Button>
                    </Form.Item>
                </Form>
            </Col>
        </Row>)
        ;
}