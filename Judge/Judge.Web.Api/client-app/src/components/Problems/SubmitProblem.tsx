import React, {useState} from "react";
import {ProblemLanguage} from "../../api/Api.ts";
import {Button, Flex, Select, Upload, UploadFile, UploadProps} from "antd";
import {UploadOutlined} from '@ant-design/icons';

export interface SubmitProblemProps {
    languages: ProblemLanguage[]
}

export const SubmitProblem: React.FC<SubmitProblemProps> = (props) => {
    const values = props.languages.map(p => ({
        value: p.id,
        label: p.name
    }));

    const [file, setFile] = useState<UploadFile | null>();
    const [languageId, setLanguageId] = useState<number | null>();

    const uploadProps: UploadProps = {
        onRemove: () => {
            setFile(null);
        },
        beforeUpload: (file) => {
            setFile(file);
            return false;
        },
        maxCount: 1
    };

    return (
        <Flex gap="small" vertical style={{width: 360}}>
            <h3>Send solution</h3>
            <Select options={values} onChange={value => setLanguageId(value)}/>
            <Upload {...uploadProps}>
                <Button icon={<UploadOutlined/>}>Select File</Button>
            </Upload>
            <Button
                type="primary"
                disabled={!file || !languageId}
            >
                Submit
            </Button>
        </Flex>
    );
}