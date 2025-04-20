import json

# Replace 'data.json' with the path to your JSON file
input_file = 'ZombieStringsTranslate.json'
output_file = 'ZombieIDs.txt'

with open(input_file, 'r', encoding='utf-8') as file:
    data = json.load(file)

seed_list = []

# Traverse the JSON structure to extract seedType and name
for plant_group in data.get("zombies", []):
    seedType = plant_group.get("theZombieType")
    name = plant_group.get("name")
    if seedType is not None and name:
        seed_list.append(f"{seedType} - {name}")

# Write the output to a file
with open(output_file, 'w', encoding='utf-8') as file:
    file.write("\n".join(seed_list))

print(f"Seed IDs have been written to {output_file}.")
