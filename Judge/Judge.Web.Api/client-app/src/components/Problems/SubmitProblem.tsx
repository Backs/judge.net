import React, {useState} from "react";
import {ProblemLanguage} from "../../api/Api.ts";
import {Button, Flex, Select, Upload, UploadProps} from "antd";
import {UploadOutlined} from '@ant-design/icons';
import {judgeApi} from "../../api/JudgeApi.ts";
import {handleError} from "../../helpers/handleError.ts";
import {AxiosError} from "axios";
import {RcFile} from "antd/es/upload";

export interface SubmitProblemProps {
    languages: ProblemLanguage[],
    problemId?: number,
    contestId?: number,
    problemLabel?: string
}

export const SubmitProblem: React.FC<SubmitProblemProps> = (props) => {
    const values = props.languages.map(p => ({
        value: p.id,
        label: p.name
    }));

    const [file, setFile] = useState<RcFile>();
    const [languageId, setLanguageId] = useState<number>();
    const [isUpdating, setUpdating] = useState<boolean>();

    const uploadProps: UploadProps = {
        onRemove: () => {
            setFile(undefined);
        },
        beforeUpload: (file) => {
            setFile(file);
            return false;
        },
        maxCount: 1
    };

    const submitSolution = async () => {
        setUpdating(true);
        const api = judgeApi();
        const data = {
            File: file!,
            ProblemId: props.problemId,
            LanguageId: languageId!,
            ProblemLabel: props.problemLabel,
            ContestId: props.contestId
        };
        
        try {
            const response = await api.api.submitsSubmitsUpdate(data)
            console.log(response);

        } catch (e: AxiosError) {
            handleError(e);
        }

        setUpdating(false);
    }

    return (
        <Flex gap="small" vertical style={{width: 360}}>
            <h3>Send solution</h3>
            <Select options={values} onChange={value => setLanguageId(value)}/>
            <Upload {...uploadProps}>
                <Button icon={<UploadOutlined/>}>Select File</Button>
            </Upload>
            <Button
                type="primary"
                disabled={!file || !languageId || isUpdating}
                onClick={submitSolution}
            >
                Submit
            </Button>
        </Flex>
    );
};