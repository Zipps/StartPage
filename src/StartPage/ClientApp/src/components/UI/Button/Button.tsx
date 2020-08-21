import React from 'react';

import classes from './Button.module.css';

interface ButtonProps {
    btnType: string;
    children: any;
    clicked?: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    disabled: boolean;
    type?: "button" | "submit" | "reset";
}

const button = (props: ButtonProps) => (
    <button
        disabled={props.disabled}
        className={[classes.Button, classes[props.btnType]].join(' ')}
        type={props.type}
        onClick={props.clicked}>{props.children}</button>
);

export default button