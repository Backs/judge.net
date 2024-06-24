import React, {useState} from "react";
import {ProblemLanguage} from "../../api/Api.ts";
import {Button, Flex, Select, Upload, UploadProps} from "antd";
import {UploadOutlined} from '@ant-design/icons';
import {judgeApi} from "../../api/JudgeApi.ts";
import {handleError} from "../../helpers/handleError.ts";
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

    const [fileList, setFileList] = useState<RcFile[]>();
    const [languageId, setLanguageId] = useState<number>();
    const [isUpdating, setUpdating] = useState<boolean>(false);

    const uploadProps: UploadProps = {
        onRemove: () => {
            setFileList([]);
        },
        beforeUpload: (file) => {
            setFileList([file]);
            return false;
        },
        maxCount: 1,
        fileList: fileList
    };

    const submitSolution = async () => {
        setUpdating(true);
        const api = judgeApi();
        const data: {
            File: File,
            LanguageId: number,
            ProblemId?: number,
            ContestId?: number,
            ProblemLabel?: string
        } = {
            File: fileList![0],
            LanguageId: languageId!
        };

        if (props.problemId) {
            data.ProblemId = props.problemId;
        }
        if (props.contestId) {
            data.ContestId = props.contestId;
        }
        if (props.problemLabel) {
            data.ProblemLabel = props.problemLabel;
        }


        try {
            await api.api.submitsSubmitsUpdate(data);
            setFileList([]);
        } catch (e: any) {
            handleError(e);
        } finally {
            setUpdating(false);
        }

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
                disabled={!fileList?.length || !languageId || isUpdating}
                onClick={submitSolution}
            >
                Submit
            </Button>
        </Flex>
    );
};