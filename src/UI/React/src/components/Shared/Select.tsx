import React, { ChangeEvent } from "react";
import { ISelectItems } from "../../utils/types";

interface SelectProps {
  options: ISelectItems[];
  style?: React.CSSProperties;
  onChange: (selectedValue: string) => void;
}

const Select: React.FC<SelectProps> = ({ options, onChange, style }) => {
  const handleSelectChange = (e: ChangeEvent<HTMLSelectElement>) => {
    onChange(e.target.value);
  };

  return (
    <select
      className="form-select text-white-dark"
      style={style}
      onChange={handleSelectChange}
    >
      <option value="">Select</option>
      {options.map((option) => (
        <option key={option.value} value={option.value}>
          {option.label}
        </option>
      ))}
    </select>
  );
};

export default Select;
