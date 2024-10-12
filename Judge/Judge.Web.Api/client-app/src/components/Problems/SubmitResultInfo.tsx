import React, {useEffect, useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import {judgeApi} from "../../api/JudgeApi.ts";
import {handleError} from "../../helpers/handleError.ts";
import {SubmitResultExtendedInfo} from "../../api/Api.ts";
import {Button, Flex, Spin, Tag} from "antd";
import Title from "antd/lib/typography/Title";
import {convertBytesToMegabytes, convertMsToSeconds} from "../../helpers/formatters.ts";
import {getColor, getStatusText} from "../../helpers/submitStatusHelper.ts";

export const SubmitResultInfo: React.FC = () => {
    const {submitResultId} = useParams();
    const [submitResult, setSubmitResult] = useState<SubmitResultExtendedInfo>();
    const navigate = useNavigate();

    const api = judgeApi();

    useEffect(() => {
        const fetchData = async () => {
            const response = await api.api.submitsDetail(Number(submitResultId));
            setSubmitResult(response.data)
        }

        fetchData().catch(e => handleError(e));
    }, [submitResultId]);

    const rejudgeSolution = async () => {
        try {
            var response = await api.api.submitsRejudgeCreate(Number(submitResultId));
            navigate(`/submit-results/${response.data.submitResultId}`);
        } catch (e: any) {
            handleError(e);
        }
    }

    return (
        !submitResult ? <Spin size="large"/> : <>
            <Title style={{textAlign: 'center'}}>{submitResult.problemName}</Title>
            <Flex gap="small" vertical>
                <div>
                    <Button type="primary" danger onClick={rejudgeSolution}>
                        Rejudge
                    </Button>
                </div>
                <div>
                    Status: <Tag bordered={false} color={getColor(submitResult.status)}>{getStatusText(submitResult.status)}</Tag>
                </div>
                <div>
                    Passed tests: {submitResult.passedTests}
                </div>
                <div>
                    Total seconds: {convertMsToSeconds(submitResult.totalMilliseconds)}
                </div>
                <div>
                    Total megabytes: {convertBytesToMegabytes(submitResult.totalBytes)}
                </div>
                <div>
                    Language: {submitResult.language}
                </div>
                <div>
                    Compiler output:
                    <pre><code>{submitResult.compilerOutput}</code></pre>
                </div>
                <div>
                    Run output:
                    <pre><code>{submitResult.runOutput}</code></pre>
                </div>
                <div>
                    Source code:
                    <pre><code>{submitResult.sourceCode}</code></pre>
                </div>
            </Flex>
        </>
    );
}