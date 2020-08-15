import * as React from 'react';

import styles from '.Bookmark.module.css';

interface BookmarkProps {
    image: string;
    title: string;
}

export default (props: BookmarkProps) => {
    <div className={styles.Bookmark}>
        <img src={props.image} alt={props.title}></img>
        <p>{props.title}</p>
    </div>
}