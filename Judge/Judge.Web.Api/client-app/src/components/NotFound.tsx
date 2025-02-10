import React from 'react';
import {Result} from 'antd';

export const NotFound: React.FC = () => (
    <Result
        status="404"
        subTitle="Sorry, the page you visited does not exist."
    />
);