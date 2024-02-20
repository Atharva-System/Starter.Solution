import React from "react";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";

interface TextEditorProps {
  value: string;
  style?: React.CSSProperties;
  onChange: (value: string) => void;
}

const TextEditor: React.FC<TextEditorProps> = ({ value, onChange, style }) => {
  return (
    <ReactQuill
      theme="snow"
      value={value}
      defaultValue={value}
      style={style}
      onChange={onChange}
    />
  );
};

export default TextEditor;
