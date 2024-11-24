import React, {useEffect, useState} from "react";
import {judgeApi} from "../../api/JudgeApi.ts";
import {TableProps} from "antd/lib/table";
import {Language} from "../../api/Api.ts";
import {Table} from "antd";
import {handleError} from "../../helpers/handleError.ts";

type ColumnTypes = Exclude<TableProps<Language>['columns'], undefined>;
export const Languages: React.FC = () => {
    const [isLoading, setLoading] = useState(true);
    const [languages, setLanguages] = useState<Language[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            const api = judgeApi();

            const result = await api.api.adminLanguagesList();

            setLanguages(result.data.items);

            setLoading(false);
        }

        fetchData().catch(e => handleError(e));
    }, []);
    
    const defaultColumns: (ColumnTypes[number] & { editable?: boolean; dataIndex: string })[] = [
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'name',
            editable: true
        },
        {
            title: 'Description',
            dataIndex: 'description',
            key: 'description'
        },
        {
            title: 'Compilable',
            dataIndex: 'isCompilable',
            key: 'isCompilable'
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
            title: 'Output file',
            dataIndex: 'outputFileTemplate',
            key: 'outputFileTemplate'
        },
        {
            title: 'Run string',
            dataIndex: 'runStringTemplate',
            key: 'runStringTemplate'
        },
        {
            title: 'Hidden',
            dataIndex: 'isHidden',
            key: 'isHidden'
        }
    ];

    const columns = defaultColumns.map((col) => {
        if (!col.editable) {
            return col;
        }
        return {
            ...col,
            onCell: (record: Language) => ({
                record,
                editable: col.editable,
                dataIndex: col.dataIndex,
                title: col.title,
                handleSave,
            }),
        };
    });

    const handleSave = (row: Language) => {
        const newData = [...languages];
        const index = newData.findIndex((item) => row.id === item.id);
        const item = newData[index];
        newData.splice(index, 1, {
            ...item,
            ...row,
        });
        setLanguages(newData);
    };

    return (
        <div>
            <Table dataSource={languages} columns={columns as ColumnTypes} pagination={false} loading={isLoading}/>
        </div>
    );
}