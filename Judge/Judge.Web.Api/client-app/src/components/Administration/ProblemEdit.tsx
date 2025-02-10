import {useNavigate, useParams} from "react-router-dom";
import React, {useEffect, useState} from "react";
import {EditProblem} from "../../api/Api.ts";
import {UserState} from "../../userSlice.ts";
import {useSelector} from "react-redux";
import {handleError} from "../../helpers/handleError.ts";
import {judgeApi} from "../../api/JudgeApi.ts";
import {Button, Form, Input, Spin, Switch} from "antd";
import TextArea from "antd/lib/input/TextArea";
import Markdown from "react-markdown";
import styles from "../../styles/Markdown.module.css";
import remarkGfm from "remark-gfm";
import rehypeRaw from "rehype-raw";
import {problemTemplate} from "../../helpers/problemsHelper.ts";

export const ProblemEdit: React.FC = () => {
    const {problemId} = useParams();
    const [problem, setProblem] = useState<EditProblem>({
        isOpened: false,
        memoryLimitBytes: 524288000,
        name: "",
        statement: problemTemplate,
        testsFolder: "",
        timeLimitMilliseconds: 1000,
        id: null
    });
    const [isLoading, setLoading] = useState(true);
    const navigate = useNavigate();
    const api = judgeApi();
    const [form] = Form.useForm()

    const {user}: UserState = useSelector((state: any) => state.user)
    const isAdmin = user?.roles.includes("admin") || false;

    if (!isAdmin) {
        navigate("/login");
    }

    useEffect(() => {
        const fetchData = async () => {
            if (problemId) {
                const response = await api.api.problemsEditableDetail(Number(problemId));
                setProblem(response.data);
                form.setFieldsValue(response.data);
            } else {
                form.setFieldsValue(problem);
            }
            setLoading(false);
        }

        fetchData().catch(e => handleError(e));
    }, [problemId, form]);

    const saveProblem = async () => {
        setLoading(true);

        const response = await api.api.problemsUpdate(problem)

        navigate(`/problems/${response.data.id}/edit`, {replace: true});

        setLoading(false);
    }


    return (
        isLoading ? <Spin size="large"/> :
            <>
                <Form
                    form={form}
                    layout="horizontal"
                    onValuesChange={(changedValues) => {
                        setProblem({...problem, ...changedValues});
                    }}
                >
                    <Form.Item label="Name" name="name">
                        <Input/>
                    </Form.Item>
                    <Form.Item label="Time limit in milliseconds" name="timeLimitMilliseconds">
                        <Input/>
                    </Form.Item>
                    <Form.Item label="Memory limit in bytes" name="memoryLimitBytes">
                        <Input/>
                    </Form.Item>
                    <Form.Item label="Test folder path" name="testsFolder">
                        <Input/>
                    </Form.Item>
                    <Form.Item label="Is opened" name="isOpened">
                        <Switch/>
                    </Form.Item>
                    <Form.Item label="Statement" name="statement">
                        <TextArea rows={20} style={{fontFamily: 'monospace'}}/>
                    </Form.Item>
                    <Form.Item>
                        <Button type="primary" onClick={saveProblem}>Save</Button>
                    </Form.Item>
                </Form>
                <Markdown
                    className={styles.markdown}
                    remarkPlugins={[remarkGfm]}
                    rehypePlugins={[rehypeRaw]}
                    remarkRehypeOptions={{allowDangerousHtml: true}}
                >{problem?.statement}</Markdown>
            </>
    );
}