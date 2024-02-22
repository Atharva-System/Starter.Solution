import React from "react";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";

interface TextEditorProps {
  value: string;
  placeholder?: string;
  style?: React.CSSProperties;
  onChange: (value: string) => void;
}

const TextEditor: React.FC<TextEditorProps> = ({
  value,
  placeholder,
  style,
  onChange,
}) => {
  return (
    <ReactQuill
      theme="snow"
      value={value}
      defaultValue={value}
      placeholder={placeholder}
      style={style}
      onChange={onChange}
    />
  );
};

export default TextEditor;
