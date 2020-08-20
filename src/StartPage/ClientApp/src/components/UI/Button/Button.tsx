import React from 'react';

import classes from './Button.module.css';

interface ButtonProps {
    btnType: string;
    children: any;
    clicked?: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    disabled: boolean;
}

const button = (props: ButtonProps) => (
    <button
        disabled={props.disabled}
        className={[classes.Button, classes[props.btnType]].join(' ')}
        onClick={props.clicked}>{props.children}</button>
);

export default button